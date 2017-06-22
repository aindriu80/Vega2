using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Vega.Controllers.Resources;
using Vega.Core;
using Vega.Core.Models;

namespace Vega.Controllers
{
    [Route("/api/vehicles/{vehicleId}/photos")]
    public class PhotosController : Controller
    {
        private readonly IHostingEnvironment _host;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly PhotoSettings _photoSettings;

        public PhotosController(IHostingEnvironment host, IVehicleRepository vehicleRepository, IPhotoRepository photoRepository,
             IMapper mapper, IOptionsSnapshot<PhotoSettings> options, IPhotoService photoService)
        {
            _photoSettings = options.Value;
            _host = host;
            _vehicleRepository = vehicleRepository;
            _photoRepository = photoRepository;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<IEnumerable<PhotoResource>> GetPhotos(int vehicleId)
        {
            var photos = await _photoRepository.GetPhotos(vehicleId);

            return _mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(int vehicleId, IFormFile file)
        {
            var vehicle = await _vehicleRepository.GetVehicle(vehicleId, includeRelated: false);
            if (vehicle == null)
                return NotFound();

            if (file == null) return BadRequest("Null file");
            if (file.Length == 0) return BadRequest("Empty file");
            if (file.Length > _photoSettings.MaxBytes) return BadRequest("Maximum file size reached");

            if (!_photoSettings.IsSupported(file.FileName)) return BadRequest("Invalid file type.");


            var uploadsFolderPath = Path.Combine(_host.WebRootPath, "uploads");
            var photo = await _photoService.UploadPhoto(vehicle, file, uploadsFolderPath);

            return Ok(_mapper.Map<Photo, PhotoResource>(photo));
        }
    }
}
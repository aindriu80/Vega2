using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Vega.Core.Models;

namespace Vega.Core
{
    public class PhotoService : IPhotoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PhotoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        async Task<Photo> IPhotoService.UploadPhoto(Vehicle vehicle, IFormFile file, string uploadsFolderPath)
        {

            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var photo = new Photo { Filename = fileName };
            vehicle.Photos.Add(photo);
            await _unitOfWork.CompleteAsync();
            return photo;
        }
    }
}
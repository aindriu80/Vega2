using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Vega.Core.Models;

namespace Vega.Core
{
    public class PhotoService : IPhotoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoStorage _photoStorage;

        public PhotoService(IUnitOfWork unitOfWork, IPhotoStorage photoStorage)
        {
            _unitOfWork = unitOfWork;
            _photoStorage = photoStorage;
        }

        public async Task<Photo> UploadPhoto(Vehicle vehicle, IFormFile file, string uploadsFolderPath)
        {

            var fileName = await _photoStorage.StorePhoto(uploadsFolderPath, file);

            var photo = new Photo { Filename = fileName };
            vehicle.Photos.Add(photo);
            await _unitOfWork.CompleteAsync();

            return photo;
        }



    }
}
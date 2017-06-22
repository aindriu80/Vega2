using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Vega.Core.Models;

namespace Vega.Core
{
    public interface IPhotoService
    {
        Task<Photo> UploadPhoto(Vehicle vehicle, IFormFile file, string uploadsFolderPath);
    }
}
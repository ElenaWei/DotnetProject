using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyDotnetProject.Core.Models;
using MyDotnetProject.Models;

namespace MyDotnetProject.Core
{
    public interface IPhotoService
    {
        Task<Photo> UploadPhoto(Vehicle vehicle, IFormFile file, string uploadsFolderPath);
    }
}
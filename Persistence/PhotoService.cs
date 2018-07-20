using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyDotnetProject.Core;
using MyDotnetProject.Core.Models;
using MyDotnetProject.Models;

namespace MyDotnetProject.Persistence
{
    public class PhotoService : IPhotoService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPhotoStorage photoStorage;
        public PhotoService(IUnitOfWork unitOfWork, IPhotoStorage photoStorage)
        {
            this.photoStorage = photoStorage;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Photo> UploadPhoto(Vehicle vehicle, IFormFile file, string uploadsFolderPath)
        {
            var fileName = await photoStorage.StorePhoto(uploadsFolderPath, file);

            // System.Drawing -by self(new in .net Core)--option
            // save to database
            var photo = new Photo { FileName = fileName };
            vehicle.Photos.Add(photo);
            await unitOfWork.Complete();

            return photo;
        }
    }
}
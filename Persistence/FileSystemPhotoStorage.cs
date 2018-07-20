using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyDotnetProject.Core;

namespace MyDotnetProject.Persistence
{
    public class FileSystemPhotoStorage : IPhotoStorage
    {
        public async Task<string> StorePhoto(string uploadsFolderPath, IFormFile file)
        {
            //host.WebRootPath ---> wwwroot folder path
            //var uploadsFolderPath = Path.Combine(host.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            // generate the file name using Guid
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;

        }
    }
}
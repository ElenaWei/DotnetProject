using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyDotnetProject.Core
{
    public interface IPhotoStorage
    {
         Task<string> StorePhoto(string uploadsFolderPath, IFormFile file); 
    }
}
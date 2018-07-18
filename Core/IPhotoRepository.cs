using System.Collections.Generic;
using System.Threading.Tasks;
using MyDotnetProject.Core.Models;

namespace MyDotnetProject.Core
{
    public interface IPhotoRepository
    {
         Task<IEnumerable<Photo>> GetPhotos(int vehicleId);
    }
}
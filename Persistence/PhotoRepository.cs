using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyDotnetProject.Core;
using MyDotnetProject.Core.Models;

namespace MyDotnetProject.Persistence
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly MyDbContext context;
        public PhotoRepository(MyDbContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Photo>> GetPhotos(int vehicleId)
        {
            return await context.Photos
              .Where(p => p.VehicleId == vehicleId)
              .ToListAsync();
        }
    }
}
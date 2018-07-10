using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyDotnetProject.Models;

namespace MyDotnetProject.Persistence
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly MyDbContext context;
        public VehicleRepository(MyDbContext context)
        {
            this.context = context;

        }
        public async Task<Vehicle> GetVehicle(int id, bool includeRelated = true)
        {
            if(!includeRelated)
            {
                return await context.Vehicles.FindAsync(id);
            }
           return await context.Vehicles
           .Include(v => v.Features).ThenInclude(vf => vf.Feature)
            .Include(v => v.Model).ThenInclude(m => m.Make)
             .SingleOrDefaultAsync(v => v.Id == id);
        }

        public void Add(Vehicle vehicle)
        {
            context.Vehicles.Add(vehicle);
        }

        public void Remove(Vehicle vehicle)
        {
            context.Remove(vehicle);
        }

        public Task<Vehicle> GetVehicle(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
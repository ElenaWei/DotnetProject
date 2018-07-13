using System.Collections.Generic;
using System.Threading.Tasks;
using MyDotnetProject.Controllers.Resources;
using MyDotnetProject.Models;

namespace MyDotnetProject.Core
{
    public interface IVehicleRepository
    {
         Task<Vehicle> GetVehicle(int id, bool includeRelated = true);
         void Add(Vehicle Vehicle);
         void Remove(Vehicle vehicle);
        Task<IEnumerable<Vehicle>> GetVehicles();
    }
}
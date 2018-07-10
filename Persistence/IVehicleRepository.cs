using System.Threading.Tasks;
using MyDotnetProject.Models;

namespace MyDotnetProject.Persistence
{
    public interface IVehicleRepository
    {
         Task<Vehicle> GetVehicle(int id, bool includeRelated = true);
         void Add(Vehicle Vehicle);
         void Remove(Vehicle vehicle);
    }
}
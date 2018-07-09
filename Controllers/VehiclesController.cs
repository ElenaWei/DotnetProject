using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDotnetProject.Controllers.Resources;
using MyDotnetProject.Models;
using MyDotnetProject.Persistence;

namespace MyDotnetProject.Controllers
{
    [Route("/api/vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IMapper mapper;
        private readonly MyDbContext context;
        public VehiclesController(IMapper mapper, MyDbContext context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpPost] // Create
        public async Task<IActionResult> CreateVehicle([FromBody] VehicleResource vehicleResource)
        {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            // mapper convert VehicleResource to Vehicle
            var vehicle = mapper.Map<VehicleResource, Vehicle>(vehicleResource);
            // set update time
            vehicle.LastUpdate = DateTime.Now;
            // add vehicle to database
            context.Vehicles.Add(vehicle);
            // save database changes
            await context.SaveChangesAsync();
            // mapper convert Vehicle to VehicleResource
            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }

        [HttpPut("{id}")] // Update( Actual API becomes to "/api/vehicle/{id}" )
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] VehicleResource vehicleResource)
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            // read the vehicle from database first
            var vehicle =  await context.Vehicles.FindAsync(id);
            if ( vehicle == null ) {
                return NotFound();
            }
            // update the vehicle using the new info from request body
            mapper.Map<VehicleResource, Vehicle>(vehicleResource, vehicle);

            vehicle.LastUpdate = DateTime.Now;
            //context.Vehicles.Add(vehicle);
            await context.SaveChangesAsync();
            
            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }

        [HttpDelete("{id}")] // Delete( Actual API becomes to "/api/vehicle/{id}" )
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            // find vehicle -> remove from database -> saveChanges -> return same id
            var vehicle = await context.Vehicles.FindAsync(id);
            if ( vehicle == null ) {
                return NotFound();
            }                
            context.Remove(vehicle);
            await context.SaveChangesAsync();
            return Ok(id);
        }

        [HttpGet("{id}")] // Delete( Actual API becomes to "/api/vehicle/{id}" )
        public async Task<IActionResult> GetVehicle(int id)
        {
            // find vehicle -> remove from database -> saveChanges -> return same id
            var vehicle = await context.Vehicles.Include(v => v.Features).SingleOrDefaultAsync(v => v.Id == id);
            if ( vehicle == null ) {
                return NotFound();
            }                
            
            var VehicleResource = mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(VehicleResource);
        }

    }
}
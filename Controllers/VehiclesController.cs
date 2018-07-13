using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDotnetProject.Controllers.Resources;
using MyDotnetProject.Models;
using MyDotnetProject.Core;
using System.Collections.Generic;

namespace MyDotnetProject.Controllers
{
    [Route("/api/vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IMapper mapper;
        private readonly IVehicleRepository repository;
        private readonly IUnitOfWork unitOfWork;
        public VehiclesController(IMapper mapper, IVehicleRepository repository, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;           
            this.mapper = mapper;
        }

        [HttpPost] // Create
        public async Task<IActionResult> CreateVehicle([FromBody] SaveVehicleResource vehicleResource)
        {           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // mapper convert VehicleResource to Vehicle
            var vehicle = mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource);
            // set update time
            vehicle.LastUpdate = DateTime.Now;
            // add vehicle to database
            // context.Vehicles.Add(vehicle);
            repository.Add(vehicle);
            // save database changes
            await unitOfWork.Complete();
            // reassign the vehicle object with all the properties inside
            vehicle = await repository.GetVehicle(vehicle.Id);
            // mapper convert Vehicle to VehicleResource
            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }

        [HttpPut("{id}")] // Update( Actual API becomes to "/api/vehicles/{id}" )
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // read the vehicle from database first
            var vehicle = await repository.GetVehicle(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            // update the vehicle using the new info from request body
            mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicle);

            vehicle.LastUpdate = DateTime.Now;

            // context.Vehicles.Add(vehicle);
            await unitOfWork.Complete();
            // update the vehicle object so that the mapping can work correctly
            vehicle = await repository.GetVehicle(vehicle.Id);
            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }

        [HttpDelete("{id}")] // Delete( Actual API becomes to "/api/vehicles/{id}" )
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            // find vehicle -> remove from database -> saveChanges -> return same id
            /*
                No need to load the completed vehicle object
                var vehicle = await context.Vehicles.FindAsync(id);
             */
            var vehicle = await repository.GetVehicle(id, includeRelated: false);
            if (vehicle == null)
            {
                return NotFound();
            }
            // context.Remove(vehicle);
            repository.Remove(vehicle);
            await unitOfWork.Complete();
            return Ok(id);
        }

        [HttpGet("{id}")] // Get( Actual API becomes to "/api/vehicles/{id}" )
        public async Task<IActionResult> GetVehicle(int id)
        {
            // find vehicle -> remove from database -> saveChanges -> return same id
            var vehicle = await repository.GetVehicle(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            var VehicleResource = mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(VehicleResource);
        }

        [HttpGet]
        public async Task<IEnumerable<VehicleResource>> GetVehicles() 
        {
            var vehicles = await repository.GetVehicles();
            
            return mapper.Map<IEnumerable<Vehicle>, IEnumerable<VehicleResource>>(vehicles);
        }

    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyDotnetProject.Controllers.Resources;
using MyDotnetProject.Core;
using MyDotnetProject.Core.Models;

namespace MyDotnetProject.Controllers
{
    // the corresponding respond URL:
    [Route("/api/vehicles/{vehicleId}/photos")]
    public class PhotosController : Controller
    {
        private readonly IHostingEnvironment host;
        private readonly IVehicleRepository repository;
        private readonly IMapper mapper;
        private readonly PhotoSettings photoSettings;
        private readonly IPhotoRepository photoRepository;
        private readonly IPhotoService photoService;
        public PhotosController(IHostingEnvironment host, 
                                IVehicleRepository repository,       
                                IPhotoService photoService, 
                                IMapper mapper, 
                                IOptionsSnapshot<PhotoSettings> options,
                                IPhotoRepository photoRepository)
        {
            this.photoService = photoService;
            this.photoSettings = options.Value;
            this.mapper = mapper;
            this.photoRepository = photoRepository;           
            this.repository = repository;
            this.host = host;
        }

    [HttpGet]
    public async Task<IEnumerable<PhotoResource>> getPhotos(int vehicleId)
    {
        var photos = await photoRepository.GetPhotos(vehicleId);

        return mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
    }

    [HttpPost]
    public async Task<IActionResult> Upload(int vehicleId, IFormFile file)
    {
        // get vehicle
        var vehicle = await repository.GetVehicle(vehicleId, includeRelated: false);
        // validation check
        if (vehicle == null)
            return NotFound();

        if (file == null)
            return BadRequest("Null file");
        if (file.Length == 0)
            return BadRequest("Empty file");
        if (file.Length > photoSettings.MaxBytes)
            return BadRequest("Max file limited");
        if (!photoSettings.IsAccepted(file.FileName))
            return BadRequest("Invalid file type.");

        var uploadsFolderPath = Path.Combine(host.WebRootPath, "uploads");
        var photo = await photoService.UploadPhoto(vehicle, file, uploadsFolderPath);

        

        return Ok(mapper.Map<Photo, PhotoResource>(photo));
    }

}
}
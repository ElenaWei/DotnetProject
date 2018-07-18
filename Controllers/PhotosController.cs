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
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly PhotoSettings photoSettings;
        private readonly IPhotoRepository photoRepository;
        public PhotosController(IHostingEnvironment host, IVehicleRepository repository,
        IUnitOfWork unitOfWork, IMapper mapper, IOptionsSnapshot<PhotoSettings> options, 
        IPhotoRepository photoRepository)
        {
            this.photoSettings = options.Value;
            this.mapper = mapper;
            this.photoRepository = photoRepository;
            this.unitOfWork = unitOfWork;
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

            //host.WebRootPath ---> wwwroot folder path
            var uploadsFolderPath = Path.Combine(host.WebRootPath, "uploads");
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

            // System.Drawing -by self(new in .net Core)

            // Save to database
            var photo = new Photo { FileName = fileName };
            vehicle.Photos.Add(photo);
            await unitOfWork.Complete();

            return Ok(mapper.Map<Photo, PhotoResource>(photo));
        }

    }
}
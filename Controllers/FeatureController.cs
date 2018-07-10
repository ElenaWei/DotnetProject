using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDotnetProject.Controllers.Resources;
using MyDotnetProject.Models;
using MyDotnetProject.Persistence;

namespace MyDotnetProject.Controllers
{
    public class FeatureController : Controller
    {
        private readonly MyDbContext context;
        private readonly IMapper mapper;
        public FeatureController(MyDbContext context, IMapper mapper) 
        {
            this.mapper = mapper;
            this.context = context;
        }
            

        [HttpGet("api/features")]
        public async Task<IEnumerable<KeyValueResource>> GetFeatures()
        {
            var features = await context.Features.ToListAsync();
            return mapper.Map<List<Feature>, List<KeyValueResource>>(features);
        }
    }
}
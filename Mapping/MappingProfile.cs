using AutoMapper;
using MyDotnetProject.Controllers.Resources;
using MyDotnetProject.Models;

namespace MyDotnetProject.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Make, MakeResource>();
            CreateMap<Model, ModelResource>();
        }
    }
}
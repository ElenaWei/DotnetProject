using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MyDotnetProject.Controllers.Resources;
using MyDotnetProject.Core.Models;
using MyDotnetProject.Models;

namespace MyDotnetProject.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to API Resource
            CreateMap(typeof(QueryResult<>), typeof(QueryResultResource<>)); // for Generic
            CreateMap<Make, MakeResource>();
            CreateMap<Make, KeyValueResource>();
            CreateMap<Model, KeyValueResource>();
            CreateMap<Feature, KeyValueResource>();
            
            //Vehicle => VehicleResource
            CreateMap<Vehicle, SaveVehicleResource>()
            .ForMember(vr => vr.Contact, opt => opt.MapFrom(v => new ContactResource {
                Name = v.ContactName, Email = v.ContactEmail, Phone = v.ContactPhone }))
            .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf => vf.FeatureId)));

            CreateMap<Vehicle, VehicleResource>()
            .ForMember(vr => vr.Make, opt => opt.MapFrom(v => v.Model.Make))
            .ForMember(vr => vr.Contact, opt => opt.MapFrom(v => new ContactResource {
                Name = v.ContactName, Email = v.ContactEmail, Phone = v.ContactPhone }))
            .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf => 
                new KeyValueResource{ Id = vf.Feature.Id, Name = vf.Feature.Name})));

            // API Resource to Domain
            CreateMap<VehicleQueryResource, VehicleQuery>();
            // VehicleResource => Vehicle
            CreateMap<SaveVehicleResource, Vehicle>()
            .ForMember(v => v.Id, opt => opt.Ignore())
            .ForMember(v => v.ContactName, opt => opt.MapFrom(vr => vr.Contact.Name))
            .ForMember(v => v.ContactEmail, opt => opt.MapFrom(vr => vr.Contact.Email))
            .ForMember(v => v.ContactPhone, opt => opt.MapFrom(vr => vr.Contact.Phone))
            .ForMember(v => v.Features, opt => opt.Ignore())
            .AfterMap((vr, v) => {
                /*// Remove unselected features
                var removedFeatures = new List<VehicleFeature>();
                foreach ( var f in v.Features ) {
                    if ( !vr.Features.Contains(f.FeatureId))
                        removedFeatures.Add(f);
                }
                foreach ( var f in removedFeatures ) {
                    v.Features.Remove(f);
                }*/

                // rewrite using LINQ (same logic as before)
                var removedFeatures = v.Features.Where(f => !vr.Features.Contains(f.FeatureId)).ToList();
                foreach ( var f in removedFeatures )
                    v.Features.Remove(f);

                /*// Add new features
                foreach ( var id in vr.Features ) {
                    if(!v.Features.Any(f => f.FeatureId == id))
                        v.Features.Add(new VehicleFeature {FeatureId = id });
                } */   

                // rewrite using LINQ (same logic as before)
                var addedFeatures = vr.Features.Where(id => !v.Features.Any(f => f.FeatureId == id))
                .Select(id => new VehicleFeature { FeatureId = id }).ToList();
                foreach ( var f in addedFeatures )
                    v.Features.Add(f);
            });
        }
    }    
}
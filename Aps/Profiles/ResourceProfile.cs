using Aps.Shared.Entity;
using Aps.Shared.Model;
using AutoMapper;

namespace Aps.Profiles
{
    public class ResourceProfile : Profile
    {
        public ResourceProfile()
        {
            CreateMap<ApsResource, ResourceDto>();
            CreateMap<ResourceClassWithResource, ResourceClassWithResourceDto>()
                .IncludeMembers(x => x.ResourceClass);

            CreateMap<ResourceClass, ResourceClassWithResourceDto>();


            CreateMap<ResourceAddDto, ApsResource>();
            CreateMap<ResourceClassWithResourceAddDto, ResourceClassWithResource>();
        }
    }
}
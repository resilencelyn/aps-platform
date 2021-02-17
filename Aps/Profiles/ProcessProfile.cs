using Aps.Shared.Entity;
using Aps.Shared.Model;
using AutoMapper;

namespace Aps.Profiles
{
    public class ProcessProfile : Profile
    {
        public ProcessProfile()
        {
            CreateMap<Workspace, string>().ConvertUsing(src => src.ToString());
            CreateMap<ProductionMode, string>().ConvertUsing(src => src.ToString());
            CreateMap<ResourceType, string>().ConvertUsing(src => src.ToString());

            CreateMap<ApsProcess, ProcessDto>()
                .Include<ApsManufactureProcess, ManufactureProcessDto>()
                .Include<ApsAssemblyProcess, AssemblyProcessDto>();

            CreateMap<ApsManufactureProcess, ManufactureProcessDto>();
            CreateMap<ApsAssemblyProcess, AssemblyProcessDto>();

            CreateMap<ApsProcessResource, ProcessResourceDto>();
        }
    }
}
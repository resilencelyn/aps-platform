using Aps.Shared.Entity;
using Aps.Shared.Extensions;
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

            #region Dto

            CreateMap<ApsProcess, ProcessDto>()
                .Include<ApsManufactureProcess, ManufactureProcessDto>()
                .Include<ApsAssemblyProcess, AssemblyProcessDto>();

            CreateMap<ApsManufactureProcess, ManufactureProcessDto>();
            CreateMap<ApsAssemblyProcess, AssemblyProcessDto>();

            CreateMap<ApsProcessResource, ProcessResourceDto>()
                .IncludeMembers(x => x.ResourceClass);
            CreateMap<ResourceClass, ProcessResourceDto>();

            CreateMap<ApsAssemblyProcessSemiProduct, AssemblyProcessSemiProductDto>();

            #endregion

            #region AddDto

            CreateMap<AssemblyProcessAddDto, ApsAssemblyProcess>();
            CreateMap<ManufactureProcessAddDto, ApsManufactureProcess>();

            CreateMap<ProcessResourceAddOrUpdateDto, ApsProcessResource>();

            CreateMap<ProcessResourceAddOrUpdateDto, ApsProcessResource>();
            CreateMap<AssemblyProcessSemiProductAddDto, ApsAssemblyProcessSemiProduct>();

            #endregion
        }
    }
}
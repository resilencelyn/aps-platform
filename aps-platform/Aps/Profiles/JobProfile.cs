using Aps.Shared.Entity;
using Aps.Shared.Model;
using AutoMapper;
using System;
using Aps.Shared.Extensions;

namespace Aps.Profiles
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<RecordState, string>().ConvertUsing(e => e.ToString());
            CreateMap<Guid, string>().ConvertUsing(e => e.ToString());

            CreateMap<ApsJob, JobDto>()
                .Include<ApsManufactureJob, JobDto>();

            CreateMap<ApsManufactureJob, JobDto>();

            CreateMap<ProductInstance, JobDto>();
            CreateMap<ApsProduct, JobDto>();

            CreateMap<ApsJob, JobDto>()
                .IncludeMembers(e => e.ApsOrder)
                .IncludeMembers(e => e.ApsProduct)
                .IncludeMembers(e => e.ProductInstance);

            CreateMap<ScheduleRecord, ScheduleRecordDto>();


            CreateMap<TimeSpan, int>().ConvertUsing(e => (int) e.TotalMinutes);
            CreateMap<int, TimeSpan>().ConvertUsing(e => TimeSpan.FromMinutes(e));
        }
    }
}
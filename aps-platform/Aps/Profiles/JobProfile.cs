using AutoMapper;
using System;
using Aps.Shared.Entity;
using Aps.Shared.Model;

namespace Aps.Profiles
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {

            CreateMap<Guid, string>().ConvertUsing(e => e.ToString());

            CreateMap<ApsJob, JobDto>();
            CreateMap<ProductInstance, JobDto>();
            CreateMap<ApsProduct, JobDto>();

            CreateMap<ApsJob, JobDto>()
                .IncludeMembers(e => e.ApsOrder)
                .IncludeMembers(e => e.ApsProduct)
                .IncludeMembers(e => e.ProductInstance);
            
            CreateMap<TimeSpan, int>().ConvertUsing(e => (int)e.TotalMinutes);
            CreateMap<int, TimeSpan>().ConvertUsing(e => TimeSpan.FromMinutes(e));
        }
    }
}
using AutoMapper;
using System;

namespace Aps.Profiles
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            // CreateMap<ApsJob, JobDto>()
            //     .IncludeMembers(e => e.ApsOrder)
            //     .IncludeMembers(e => e.ApsProduct)
            //     .IncludeMembers(e => e.ProductInstance);
            //
            CreateMap<TimeSpan, int>().ConvertUsing(e => (int)e.TotalMinutes);
            CreateMap<int, TimeSpan>().ConvertUsing(e => TimeSpan.FromMinutes(e));
        }
    }
}
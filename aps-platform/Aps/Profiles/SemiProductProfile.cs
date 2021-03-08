using Aps.Services;
using Aps.Shared.Entity;
using Aps.Shared.Model;
using AutoMapper;

namespace Aps.Profiles
{
    public class SemiProductProfile : Profile
    {
        public SemiProductProfile()
        {
            CreateMap<ApsSemiProduct, SemiProductDto>();

            CreateMap<SemiProductAddOrUpdateDto, ApsSemiProduct>();
        }
    }
}
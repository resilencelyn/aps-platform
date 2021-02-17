using Aps.Shared.Entity;
using Aps.Shared.Model;
using AutoMapper;

namespace Aps.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ApsProduct, ProductDto>();
            CreateMap<ApsProductSemiProduct, ProductSemiProductDto>();
        }
    }
}
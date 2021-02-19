using Aps.Shared.Entity;
using Aps.Shared.Model;
using AutoMapper;

namespace Aps.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ApsProduct, ProductDto>().ReverseMap();
            CreateMap<ApsProductSemiProduct, ProductSemiProductDto>();

            CreateMap<ProductAddDto, ApsProduct>();
            CreateMap<ProductUpdateDto, ApsProduct>();

            CreateMap<ProductSemiProductAddDto, ApsProductSemiProduct>();
            CreateMap<ProductSemiProductUpdateDto, ApsProductSemiProduct>();
        }
    }
}
using Aps.Shared.Entity;
using Aps.Shared.Model;
using AutoMapper;

namespace Aps.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<ApsOrder, OrderDto>();


            CreateMap<OrderAddDto, ApsOrder>()
                .ForMember(dest => dest.Product,
                    opt => opt.MapFrom(src => new ApsProduct {Id = src.ProductId}));

            CreateMap<OrderUpdateDto, ApsOrder>()
                .ForMember(dest => dest.Product,
                    opt => opt.MapFrom(src => new ApsProduct {Id = src.ProductId}));
        }
    }
}
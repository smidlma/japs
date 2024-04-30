using AutoMapper;
using jAPS.API.Models;

namespace jAPS.API.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<OrderItem, ProductDto>()
                      .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                      .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                      .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

            CreateMap<Transaction, Basket>()
            .ForMember(dest => dest.BasketId, opt => opt.MapFrom(src => src.BasketId))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Order.OrderItems.Select(orderItem =>
                new ProductDto
                {
                    ProductId = orderItem.ProductId,
                    Product = orderItem.Product,
                    Quantity = orderItem.Quantity
                })))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Order.TotalPrice));

        }
    }
}


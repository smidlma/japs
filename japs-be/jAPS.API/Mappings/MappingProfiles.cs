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
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => MapOrderItemsToProductDto(src.Order.OrderItems)))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Order.TotalPrice));

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus));

            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.PaymentProvider, opt => opt.MapFrom(src => src.PaymentProvider))
                .ForMember(dest => dest.TransactionState, opt => opt.MapFrom(src => src.TransactionState));

            CreateMap<Transaction, OrderDetail>()
            .ForMember(dest => dest.TransactionDto, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.OrderDto, opt => opt.MapFrom(src => src.Order))
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.Basket, opt => opt.MapFrom(src => src))
            .ForPath(dest => dest.Basket.Items, opt => opt.MapFrom(src => MapOrderItemsToProductDto(src.Order.OrderItems)))
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer));
        }

        private List<ProductDto> MapOrderItemsToProductDto(IEnumerable<OrderItem> orderItems)
        {
            return orderItems.Select(orderItem => new ProductDto
            {
                ProductId = orderItem.ProductId,
                Product = orderItem.Product,
                Quantity = orderItem.Quantity
            }).ToList();            
        }
    }
}


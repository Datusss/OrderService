using Application.UseCases.OrderProduct.DTOs;
using AutoMapper;
using Domain.OrderProduct.Aggregates;

namespace Application.Mappers;

public class MapOrderItemToOrderItemDTO: Profile
{
    public MapOrderItemToOrderItemDTO()
    {
        CreateMap<OrderItem, OrderItemDTO>().ForMember(dest => dest.Name, 
            opt => opt.MapFrom(src => src.Product.Name));
    }
}
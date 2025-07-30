using Application.UseCases.OrderProduct.DTOs;
using AutoMapper;
using Domain.OrderProduct.Aggregates;

namespace Application.Mappers;

public class MapOrderToOrderDTO: Profile
{
    public MapOrderToOrderDTO()
    {
        CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.CustomerName, 
                opt => opt.MapFrom(src => src.Customer == null ? null : src.Customer.Name))
            .ForMember(dest => dest.Total, 
                opt => opt.MapFrom(src => src.OrderItems.Sum(x => x.Quantity * x.Price)))
            .ForMember(dest => dest.TotalQty, 
                opt => opt.MapFrom(src => src.OrderItems.Sum(x => x.Quantity)));
    }
}
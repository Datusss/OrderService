using Application.UseCases.OrderProduct.DTOs;
using AutoMapper;
using Domain.OrderProduct.Aggregates;

namespace Application.Mappers;

public class MapProductToDTO: Profile
{
    public MapProductToDTO()
    {
        CreateMap<Product, ProductDTO>();
    }
}
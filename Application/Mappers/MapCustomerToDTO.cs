using Application.UseCases.Customer.DTOs;
using AutoMapper;
using Domain.Customer.Aggregates;

namespace Application.Mappers;

public class MapCustomerToDTO: Profile
{
    public MapCustomerToDTO()
    {
        CreateMap<Customer, CustomerDTO>();
    }
}
using Application.Mappers;
using Application.UseCases.OrderProduct.Commands;
using Application.UseCases.OrderProduct.Validators;
using Domain.OrderProduct.Events;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ApplicationInjectorExtension
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<OrderBaseValidator>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateOrderCommand).Assembly, typeof(AdjustProductQtyAfterCompleteOrderDomainEvent).Assembly));
        services.AddAutoMapper(cfg => { }, typeof(MapCustomerToDTO));
    }
}
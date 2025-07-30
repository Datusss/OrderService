using Domain.OrderProduct.Aggregates;

namespace Application.UseCases.OrderProduct.DTOs;

public record AddOrderItemToOrderDTO(Guid OrderId, ProductDTO Product, int RequiringQuantity);
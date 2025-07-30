using Application.UseCases.OrderProduct.DTOs;
using Domain.OrderProduct.Aggregates;
using Domain.OrderProduct.Repositories;
using MediatR;

namespace Application.UseCases.OrderProduct.Commands;

public class CreateOrderCommand(CreateOrUpdateOrderDTO payload): IRequest<Order>
{
    public CreateOrUpdateOrderDTO Payload { get; } = payload;
}

public class CreateOrderCommandHandler(IOrderCommandRepository orderRepository)
    : IRequestHandler<CreateOrderCommand, Order>
{
    public async Task<Order> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = new Order(
            Guid.NewGuid().ToString(),
            command.Payload.CustomerId
            );
        try
        {
            await orderRepository.BeginTransactionAsync(cancellationToken);
            await orderRepository.CreateAsync(order, cancellationToken);
            await orderRepository.CommitAsync(null, cancellationToken);
            return order;
        }
        catch
        {
            await orderRepository.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
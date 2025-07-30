using Application.UseCases.OrderProduct.DTOs;
using Application.UseCases.OrderProduct.Validators;
using AutoMapper;
using Domain.OrderProduct.Repositories;
using FluentValidation;
using MediatR;

namespace Application.UseCases.OrderProduct.Commands;

public class AddOrderItemToOrderCommand(AddOrderItemToOrderDTO payload) : IRequest<OrderDTO>
{
    public AddOrderItemToOrderDTO Payload { get; } = payload;
}

public class AddOrderItemToOrderCommandHandler(IOrderCommandRepository orderCommandRepository, IOrderItemCommandRepository orderItemCommandRepository, IMapper mapper)
    :IRequestHandler<AddOrderItemToOrderCommand, OrderDTO>
{
    public async Task<OrderDTO> Handle(AddOrderItemToOrderCommand command, CancellationToken cancellationToken)
    {
        var validator = new OrderBaseValidator(command.Payload.RequiringQuantity);
        var validationResult = await validator.ValidateAsync(command.Payload, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var existingOrder = await orderCommandRepository.FindByIdAsync(command.Payload.OrderId, cancellationToken);
        if (existingOrder is null)
        {
            throw new ValidationException("Order not found.");
        }

        existingOrder.AddOrderItem(command.Payload.OrderId, command.Payload.RequiringQuantity,
            command.Payload.Product.Price, command.Payload.Product.Id);
        
        try
        {
            await orderItemCommandRepository.BeginTransactionAsync(cancellationToken);
            await orderItemCommandRepository.CreateAsync(existingOrder.OrderItems.First(), cancellationToken);
            orderCommandRepository.Update(existingOrder);
            await orderItemCommandRepository.CommitAsync(existingOrder, cancellationToken);
            return mapper.Map<OrderDTO>(existingOrder);
        }
        catch
        {
            await orderItemCommandRepository.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
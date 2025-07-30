using Domain.OrderProduct.Events;
using Domain.OrderProduct.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.OrderProduct.DomainEventHandlers;

public class AdjustProductQtyAfterCompleteOrderDomainEventHandler(IProductCommandRepository productCommandRepository, ILogger<AdjustProductQtyAfterCompleteOrderDomainEventHandler> logger)
    : INotificationHandler<AdjustProductQtyAfterCompleteOrderDomainEvent>
{
    public async Task Handle(AdjustProductQtyAfterCompleteOrderDomainEvent request, CancellationToken cancellationToken)
    {
        var existingProduct = await productCommandRepository.FindByIdAsync(request.ProductId, cancellationToken);
        if (existingProduct is null)
        {
            logger.LogWarning($"Product {request.ProductId} not found");
            return;
        }
        
        existingProduct.ReduceQuantity(request.Quantity);
        try
        {
            await productCommandRepository.BeginTransactionAsync(cancellationToken);
            productCommandRepository.Update(existingProduct);
            await productCommandRepository.CommitAsync(null, cancellationToken);
        }
        catch
        {
            await productCommandRepository.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
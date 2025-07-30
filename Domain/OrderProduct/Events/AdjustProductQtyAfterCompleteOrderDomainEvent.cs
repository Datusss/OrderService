using MediatR;

namespace Domain.OrderProduct.Events;

public record AdjustProductQtyAfterCompleteOrderDomainEvent(int ProductId, int Quantity): INotification;
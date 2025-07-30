using Domain.OrderProduct.Aggregates;
using Domain.OrderProduct.Repositories;
using MediatR;

namespace Infrastructure.Persistence.EF.Repositories;

public class OrderItemEFRepository(AppDbContext dbContext, IMediator mediator)
    : EfRepository<OrderItem>(dbContext, mediator), IOrderItemCommandRepository
{
    
}
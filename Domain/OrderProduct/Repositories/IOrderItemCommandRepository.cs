using Domain.Abstractions;
using Domain.OrderProduct.Aggregates;

namespace Domain.OrderProduct.Repositories;

public interface IOrderItemCommandRepository: IBaseCommandRepository<OrderItem>
{
    
}
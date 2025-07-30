using Domain.Abstractions;
using Domain.OrderProduct.Aggregates;

namespace Domain.OrderProduct.Repositories;

public interface IOrderCommandRepository: IBaseCommandRepository<Order>
{
    Task<bool> AnyByNameAsync(string name, Guid? id, CancellationToken cancellationToken);
}
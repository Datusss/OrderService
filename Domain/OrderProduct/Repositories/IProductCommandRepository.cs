using Domain.Abstractions;
using Domain.OrderProduct.Aggregates;

namespace Domain.OrderProduct.Repositories;

public interface IProductCommandRepository: IBaseCommandRepository<Product>
{
    Task<bool> AnyAsync(CancellationToken cancellationToken);
}
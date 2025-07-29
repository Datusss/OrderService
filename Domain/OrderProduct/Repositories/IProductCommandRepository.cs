using Domain.Abstractions;
using Domain.OrderProduct.Aggregates;

namespace Domain.OrderProduct.Repositories;

public interface IProductCommandRepository: IBaseCommandRepository<Product, int>
{
    Task<Product?> FindByNameAsync(string name, CancellationToken cancellationToken);
    Task<bool> AnyAsync(CancellationToken cancellationToken);
}
using Domain.OrderProduct.Aggregates;

namespace Domain.OrderProduct.Repositories;

public interface IProductQueryRepository
{
    Task<Product?> FindByNameAsync(string name, CancellationToken cancellationToken);
}
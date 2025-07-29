using Domain.OrderProduct.Aggregates;
using Domain.OrderProduct.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EF.Repositories;

public class ProductEFRepository(AppDbContext dbContext)
    : EfRepository<Product, int>(dbContext), IProductCommandRepository
{
    public async Task<Product?> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await FindByQueryableAsync(DbSet.Where(e => e.Name == name), cancellationToken);
    }

    public async Task<bool> AnyAsync(CancellationToken cancellationToken)
    {
        return await DbSet.AnyAsync(cancellationToken);
    }
}
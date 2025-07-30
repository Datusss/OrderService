using Domain.OrderProduct.Aggregates;
using Domain.OrderProduct.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EF.Repositories;

public class ProductEFRepository(AppDbContext dbContext, IMediator mediator)
    : EfRepository<Product>(dbContext, mediator), IProductCommandRepository
{
    public async Task<bool> AnyAsync(CancellationToken cancellationToken)
    {
        return await DbSet.AnyAsync(cancellationToken);
    }
}
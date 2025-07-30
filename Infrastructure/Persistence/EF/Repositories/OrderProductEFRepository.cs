using Domain.OrderProduct.Aggregates;
using Domain.OrderProduct.Repositories;
using MediatR;

namespace Infrastructure.Persistence.EF.Repositories;

public class OrderProductEFRepository(AppDbContext dbContext, IMediator mediator)
    : EfRepository<Order>(dbContext, mediator), IOrderCommandRepository
{
    public async Task<bool> AnyByNameAsync(string name, Guid? id, CancellationToken cancellationToken)
    {
        return await AnyByQueryableAsync(DbSet.Where(x => x.Name == name && x.Id != id), cancellationToken);
    }
}
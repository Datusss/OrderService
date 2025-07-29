using Domain.Customer.Aggregates;
using Domain.Customer.Repositories;

namespace Infrastructure.Persistence.EF.Repositories;

public class CustomerEFRepository(AppDbContext dbContext)
    : EfRepository<Customer, int>(dbContext), ICustomerCommandRepository
{
    public async Task<Customer?> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await FindByQueryableAsync(DbSet.Where(e => e.Name == name), cancellationToken);
    }
}
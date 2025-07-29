using Domain.Abstractions;

namespace Domain.Customer.Repositories;

public interface ICustomerCommandRepository: IBaseCommandRepository<Aggregates.Customer, int>
{
    Task<Aggregates.Customer?> FindByNameAsync(string name, CancellationToken cancellationToken);
}

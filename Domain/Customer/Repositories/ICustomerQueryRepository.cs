namespace Domain.Customer.Repositories;

public interface ICustomerQueryRepository
{
    Task<Aggregates.Customer?> FindByNameAsync(string name, CancellationToken cancellationToken);
}
using Domain.Abstractions;

namespace Domain.Customer.Repositories;

public interface ICustomerCommandRepository: IBaseCommandRepository<Aggregates.Customer>
{
}

using Domain.Customer.Aggregates;
using Domain.Customer.Repositories;
using MediatR;

namespace Infrastructure.Persistence.EF.Repositories;

public class CustomerEFRepository(AppDbContext dbContext, IMediator mediator)
    : EfRepository<Customer>(dbContext, mediator), ICustomerCommandRepository
{
}
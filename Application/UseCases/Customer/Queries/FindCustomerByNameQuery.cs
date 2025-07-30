using Application.UseCases.Customer.DTOs;
using AutoMapper;
using Domain.Customer.Repositories;
using MediatR;

namespace Application.UseCases.Customer.Queries;

public class FindCustomerByNameQuery(string? name): IRequest<CustomerDTO?>
{
    public string? Name { get; } = name;
}

public class FindCustomerByNameQueryHandler(ICustomerQueryRepository customerQueryRepository, IMapper mapper)
    : IRequestHandler<FindCustomerByNameQuery, CustomerDTO?>
{
    public async Task<CustomerDTO?> Handle(FindCustomerByNameQuery query, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query.Name))
        {
            return null;
        }
        var existingCustomer = await customerQueryRepository.FindByNameAsync(query.Name, cancellationToken);
        return mapper.Map<CustomerDTO>(existingCustomer);
    }
}
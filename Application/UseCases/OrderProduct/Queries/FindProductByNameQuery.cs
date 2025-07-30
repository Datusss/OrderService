using Application.UseCases.OrderProduct.DTOs;
using AutoMapper;
using Domain.OrderProduct.Repositories;
using MediatR;

namespace Application.UseCases.OrderProduct.Queries;

public class FindProductByNameQuery(string? name): IRequest<ProductDTO?>
{
    public string? Name { get; } = name;
}

public class FindProductByNameQueryHandler(IProductQueryRepository productQueryRepository, IMapper mapper)
    : IRequestHandler<FindProductByNameQuery, ProductDTO?>
{
    public async Task<ProductDTO?> Handle(FindProductByNameQuery query, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query.Name))
        {
            return null;
        }
        var existingProduct = await productQueryRepository.FindByNameAsync(query.Name, cancellationToken);
        return mapper.Map<ProductDTO>(existingProduct);
    }
}
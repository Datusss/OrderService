using Domain.Customer.Aggregates;
using Domain.Customer.Repositories;
using Domain.OrderProduct.Aggregates;
using Domain.OrderProduct.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.EF;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var productRepository = serviceProvider.GetRequiredService<IProductCommandRepository>();
        var customerRepository = serviceProvider.GetRequiredService<ICustomerCommandRepository>();
        if (await productRepository.AnyAsync(cancellationToken))
        {
            return;
        }
        await productRepository.BeginTransactionAsync(cancellationToken);
        try
        {
            var products = new List<Product>()
            {
                new ("Pen", 10, 10000),
                new ("Book", 8, 20000),
                new ("Paper", 20, 3000)
            };
            foreach (var product in products)
            {
                await productRepository.CreateAsync(product, cancellationToken);
            }

            var customers = new List<Customer>()
            {
                new("Lasi"),
                new("Max")
            };
            foreach (var customer in customers)
            {
                await customerRepository.CreateAsync(customer, cancellationToken);
            }

            await productRepository.CommitAsync(cancellationToken);
        }
        catch
        {
            await productRepository.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
using Domain.Customer.Aggregates;
using Domain.OrderProduct.Aggregates;
using Infrastructure.Persistence.EF;
using Infrastructure.Persistence.EF.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests;

public class UnitOfWorkTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;
        var context = new AppDbContext(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task CommitAsync_SavesProductsAndCustomers_Atomically()
    {
        await using var context = GetInMemoryDbContext();
        using var productRepository = new ProductEFRepository(context);
        var customerRepository = new CustomerEFRepository(context);

        await productRepository.BeginTransactionAsync();
        var customer = new Customer( "John");
        await customerRepository.CreateAsync(customer, CancellationToken.None);
        var product = new Product("Pen", 10, 10000);
        await productRepository.CreateAsync(product, CancellationToken.None);
        await productRepository.CommitAsync();

        var savedCustomer = await customerRepository.FindByNameAsync("John", CancellationToken.None);
        var savedProduct = await productRepository.FindByNameAsync("Pen", CancellationToken.None);

        Assert.NotNull(savedCustomer);
        Assert.NotNull(savedProduct);
        Assert.Equal(10, savedProduct.Quantity);
    }

    [Fact]
    public async Task RollbackAsync_DiscardsChanges()
    {
        await using var context = GetInMemoryDbContext();
        using var customerRepository = new CustomerEFRepository(context);

        await customerRepository.BeginTransactionAsync();
        var customer = new Customer("John");
        await customerRepository.CreateAsync(customer, CancellationToken.None);
        await customerRepository.RollbackAsync();

        var savedCustomer = await customerRepository.FindByNameAsync("John", CancellationToken.None);
        Assert.Null(savedCustomer);
    }

}
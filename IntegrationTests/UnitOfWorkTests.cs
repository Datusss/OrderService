using System.Data;
using Domain.Customer.Aggregates;
using Domain.OrderProduct.Aggregates;
using Infrastructure.Persistence.Dapper.Repositories;
using Infrastructure.Persistence.EF;
using Infrastructure.Persistence.EF.Repositories;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

namespace IntegrationTests;

public class UnitOfWorkTests
{
    private readonly string _connectionString = "Data Source=test.db";
    private readonly Mock<IMediator> _mediator;

    public UnitOfWorkTests()
    {
        _mediator = new Mock<IMediator>();
    }
    
    private IDbConnection GetDapperConnection()
    {
        return new SqliteConnection(_connectionString);
    }

    private AppDbContext GetEFDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connectionString)
            .LogTo(Console.WriteLine, [DbLoggerCategory.Database.Command.Name], LogLevel.Information)
            .EnableSensitiveDataLogging()
            .Options;
        var context = new AppDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task CommitAsync_SavesProductsAndCustomers_Atomically()
    {
        await using var context = GetEFDbContext();
        using var productRepository = new ProductEFRepository(context, _mediator.Object);
        var customerRepository = new CustomerEFRepository(context, _mediator.Object);

        await productRepository.BeginTransactionAsync();
        var customer = new Customer( "John");
        await customerRepository.CreateAsync(customer, CancellationToken.None);
        var product = new Product("Pen", 10, 10000);
        await productRepository.CreateAsync(product, CancellationToken.None);
        await productRepository.CommitAsync(null, CancellationToken.None);

        var dapperConnection = GetDapperConnection();
        var customerQueryRepository = new CustomerDapperRepository(dapperConnection);
        var savedCustomer = await customerQueryRepository.FindByNameAsync("John", CancellationToken.None);
        var productQueryRepository = new ProductDapperRepository(dapperConnection);
        var savedProduct = await productQueryRepository.FindByNameAsync("Pen", CancellationToken.None);
        dapperConnection.Close();

        Assert.NotNull(savedCustomer);
        Assert.NotNull(savedProduct);
        Assert.Equal(10, savedProduct.Quantity);
    }

    [Fact]
    public async Task RollbackAsync_DiscardsChanges()
    {
        await using var context = GetEFDbContext();
        using (var customerRepository = new CustomerEFRepository(context, _mediator.Object))
        {
            await customerRepository.BeginTransactionAsync();
            var customer = new Customer("John");
            await customerRepository.CreateAsync(customer, CancellationToken.None);
            await customerRepository.RollbackAsync();
        }

        var dapperConnection = GetDapperConnection();
        var customerQueryRepository = new CustomerDapperRepository(dapperConnection);
        var savedCustomer = await customerQueryRepository.FindByNameAsync("John", CancellationToken.None);
        dapperConnection.Close();
        Assert.Null(savedCustomer);
    }

}
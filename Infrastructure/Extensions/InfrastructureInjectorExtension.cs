using System.Data;
using Domain.Customer.Repositories;
using Domain.OrderProduct.Repositories;
using Infrastructure.Persistence.Dapper.Repositories;
using Infrastructure.Persistence.EF;
using Infrastructure.Persistence.EF.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Extensions;

public static class InfrastructureInjectorExtension
{
    public static void AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(connectionString);
            // options.LogTo(Console.WriteLine, [DbLoggerCategory.Database.Command.Name], LogLevel.Information);
            // options.EnableSensitiveDataLogging();
        });
        services.AddScoped<IProductCommandRepository, ProductEFRepository>();
        services.AddScoped<ICustomerCommandRepository, CustomerEFRepository>();
        services.AddScoped<IOrderCommandRepository, OrderProductEFRepository>();
        services.AddScoped<IOrderItemCommandRepository, OrderItemEFRepository>();

        services.AddScoped<IDbConnection>(s => new SqliteConnection(connectionString));
        services.AddScoped<ICustomerQueryRepository, CustomerDapperRepository>();
        services.AddScoped<IProductQueryRepository, ProductDapperRepository>();
        
    }
}
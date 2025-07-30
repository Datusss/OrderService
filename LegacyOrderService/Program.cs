using Infrastructure.Extensions;
using Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Application.Extensions;
using Application.UseCases.Customer.Queries;
using Application.UseCases.OrderProduct.Commands;
using Application.UseCases.OrderProduct.DTOs;
using Application.UseCases.OrderProduct.Queries;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LegacyOrderService
{
    public class Program
    {
        private static readonly CancellationTokenSource cts = new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to Order Processor!");
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                Console.WriteLine("Cancel event triggered");
                cts.Cancel();
                eventArgs.Cancel = true;
            };

            var services = new ServiceCollection();
            services.AddLogging(configure => configure.AddConsole());
            services.AddInfrastructureServices("Data Source=orders.db");
            services.AddApplicationServices();
            var provider = services.BuildServiceProvider();
            
            using (var scope = provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await dbContext.Database.MigrateAsync();
                await SeedData.Initialize(scope.ServiceProvider, cts.Token);
            }

            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<Program>();
            try
            {
                using var scope = provider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                Console.WriteLine("Enter customer name(Leave it blank if the customer is missing):");
                var customerName = Console.ReadLine();
                var customer = await mediator.Send(new FindCustomerByNameQuery(customerName));
                Console.WriteLine("Saving order to database...");
                var createdOrder = await mediator.Send(new CreateOrderCommand(new CreateOrUpdateOrderDTO(null, customer?.Id)));
                Console.WriteLine("Done.");

                var product = await FindValidProductRecursiveAsync(mediator);
                Console.WriteLine("Enter quantity:");
                var qty = Convert.ToInt32(Console.ReadLine());
                var completedOrder =
                    await mediator.Send(
                        new AddOrderItemToOrderCommand(new AddOrderItemToOrderDTO(createdOrder.Id, product, qty)));
                Console.WriteLine("Processing order...");
                
                Console.WriteLine("Order complete!");
                Console.WriteLine("Customer: " + completedOrder.CustomerName);
                Console.WriteLine("Product: " + product.Name);
                Console.WriteLine("Quantity: " + completedOrder.TotalQty);
                Console.WriteLine("Total: $" + completedOrder.Total);
            }
            catch (ValidationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error: {ex.Message}"); 
                logger.LogError($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    logger.LogError($"Inner Exception: {ex.InnerException.Message}");
                    logger.LogError($"Inner Stack Trace: {ex.InnerException.StackTrace}");
                }
            }
        }

        private static async Task<ProductDTO> FindValidProductRecursiveAsync(IMediator mediator)
        {
            while (true)
            {
                Console.WriteLine("Enter product name:");
                var productName = Console.ReadLine();
                var product = await mediator.Send(new FindProductByNameQuery(productName));

                if (product is not null) return product;
                Console.WriteLine("Product not found.");
            }
        }
    }
}

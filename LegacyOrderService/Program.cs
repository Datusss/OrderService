using Infrastructure.Extensions;
using Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LegacyOrderService
{
    public static class Program
    {
        private static readonly CancellationTokenSource cts = new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                Console.WriteLine("Cancel event triggered");
                cts.Cancel();
                eventArgs.Cancel = true;
            };

            var services = new ServiceCollection();
            services.AddInfrastructureServices("Data Source=orders.db");
            var provider = services.BuildServiceProvider();
            
            using (var scope = provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await dbContext.Database.EnsureCreatedAsync();
                await dbContext.Database.MigrateAsync();
                await SeedData.Initialize(scope.ServiceProvider, cts.Token);
            }
            
            // Console.WriteLine("Welcome to Order Processor!");
            // Console.WriteLine("Enter customer name:");
            // string name = Console.ReadLine();
            //
            // Console.WriteLine("Enter product name:");
            // string product = Console.ReadLine();
            // var productRepo = new ProductRepository();
            // double price = productRepo.GetPrice(product);
            //
            //
            // Console.WriteLine("Enter quantity:");
            // int qty = Convert.ToInt32(Console.ReadLine());
            //
            // Console.WriteLine("Processing order...");
            //
            // Order order = new Order();
            // order.CustomerName = name;
            // order.ProductName = product;
            // order.Quantity = qty;
            // order.Price = 10.0;
            //
            // double total = order.Quantity * order.Price;
            //
            // Console.WriteLine("Order complete!");
            // Console.WriteLine("Customer: " + order.CustomerName);
            // Console.WriteLine("Product: " + order.ProductName);
            // Console.WriteLine("Quantity: " + order.Quantity);
            // Console.WriteLine("Total: $" + price);
            //
            // Console.WriteLine("Saving order to database...");
            // var repo = new OrderRepository();
            // repo.Save(order);
            // Console.WriteLine("Done.");
        }
    }
}

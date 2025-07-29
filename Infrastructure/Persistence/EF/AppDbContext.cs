using Domain.Customer.Aggregates;
using Domain.OrderProduct.Aggregates;
using Infrastructure.Persistence.EF.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EF;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<Order> Orders { get; private set; }
    public DbSet<OrderItem> OrderItems { get; private set; }
    public DbSet<Product> Products { get; private set; }
    public DbSet<Customer> Customers { get; private set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
using Domain.OrderProduct.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EF.EntityConfigurations;

public class OrderItemEntityTypeConfiguration: IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        
        builder.ToTable(nameof(OrderItem));
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Price)
            .IsRequired();
        builder.Property(o => o.Quantity)
            .IsRequired();
        
        builder.HasOne(o => o.Product)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
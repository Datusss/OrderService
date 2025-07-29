using Domain.OrderProduct.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EF.EntityConfigurations;

public class OrderEntityTypeConfiguration: IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        
        builder.ToTable(nameof(Order));
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(260);
        builder.HasIndex(o => o.Name).IsUnique();
        
        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(o => o.Customer)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
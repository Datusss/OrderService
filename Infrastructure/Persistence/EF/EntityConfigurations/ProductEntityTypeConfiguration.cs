using Domain.OrderProduct.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EF.EntityConfigurations;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(nameof(Product));
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Price)
            .IsRequired();
        builder.Property(o => o.Quantity)
            .IsRequired();
        builder.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(260);
        builder.HasIndex(o => o.Name).IsUnique();
    }
}
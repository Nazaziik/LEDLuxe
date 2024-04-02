using LEDLuxe.Core.Entities.Orders;
using LEDLuxe.Core.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LEDLuxe.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(49);
        builder.Property(p => p.Description).HasMaxLength(999);
        builder.Property(p => p.Price).IsRequired();
        builder.Property(p => p.StockQuantity).IsRequired().HasDefaultValue(0);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder
            .HasMany(p => p.Categories)
            .WithMany(c => c.Products)
            .UsingEntity(j => j.ToTable("ProductCategories"));

        builder
            .HasMany(p => p.Rates)
            .WithOne(c => c.Product)
            .HasForeignKey(r => r.ProductId);

        builder
            .HasMany<OrderItem>()
            .WithOne()
            .HasForeignKey(oi => oi.ProductId);
    }
}
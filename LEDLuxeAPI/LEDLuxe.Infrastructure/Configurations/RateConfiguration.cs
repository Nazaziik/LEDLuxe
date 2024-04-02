using LEDLuxe.Core.Entities.Products;
using LEDLuxe.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LEDLuxe.Infrastructure.Configurations;

public class RateConfiguration : IEntityTypeConfiguration<Rate>
{
    public void Configure(EntityTypeBuilder<Rate> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Value).IsRequired();
        builder.Property(r => r.Comment).HasMaxLength(400);

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(r => r.UserId);

        builder
            .HasOne(r => r.Product)
            .WithMany(p => p.Rates)
            .HasForeignKey(r => r.ProductId);
    }
}
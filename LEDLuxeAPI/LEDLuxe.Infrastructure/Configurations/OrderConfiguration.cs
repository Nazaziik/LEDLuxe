using LEDLuxe.Core.Entities.Orders;
using LEDLuxe.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LEDLuxe.Infrastructure.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.OrderDate);

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(o => o.UserId);

        builder
            .HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId);
    }
}
﻿using LEDLuxe.Core.Entities.Orders;
using LEDLuxe.Core.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LEDLuxe.Infrastructure.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.UnitPrice).IsRequired();
        builder.Property(oi => oi.Quantity).IsRequired().HasDefaultValue(1);

        builder
            .HasOne<Order>()
            .WithMany(o => o.OrderItems)
            .HasForeignKey(o => o.OrderId);

        builder
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(o => o.ProductId);
    }
}
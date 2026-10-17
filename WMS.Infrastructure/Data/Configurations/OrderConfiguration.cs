using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Core.Entities;

namespace WMS.Infrastructure.Data.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Status)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(p => p.ShippingAddress)
                .HasMaxLength(500);

            builder.Property(p => p.PaymentMethod)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(p => p.Currency)
                .HasConversion<string>()
                .HasMaxLength(10);

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.InvoiceNumber)
                .HasMaxLength(16);

            builder.HasIndex(p => p.InvoiceNumber)
                .IsUnique();
            builder.HasIndex(
                p => new { p.CustomerId, p.CreatedAt });

            builder.HasOne(p => p.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Items)
                .WithOne(o => o.Order)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

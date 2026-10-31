using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Core.Entities;

namespace WMS.Infrastructure.Data.Configurations
{
    public class WarehouseTaskConfiguration : IEntityTypeConfiguration<WarehouseTask>
    {
        public void Configure(EntityTypeBuilder<WarehouseTask> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Barcode)
                .IsUnicode(false)
                .HasMaxLength(13)
                .IsFixedLength();

            builder.Property(w => w.Quantity)
                .HasPrecision(18, 3);

            builder.Property(w => w.Source)
                .HasConversion<string>()
                .HasMaxLength(32);
            builder.Property(w => w.Status)
                .HasConversion<string>()
                .HasMaxLength(32);
            builder.Property(w => w.StockStatus)
                .HasConversion<string>()
                .HasMaxLength(32);

            builder.HasOne(w => w.GoodsReceipt)
                .WithMany()
                .HasForeignKey(w => w.GoodsReceiptId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(w => w.Order).WithMany()
                .HasForeignKey(w => w.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(w => w.Location).WithMany()
                .HasForeignKey(w => w.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(w => new { w.StockStatus, w.CreatedAt});
            builder.HasIndex(w => w.GoodsReceiptId);
            builder.HasIndex(w => w.OrderId);
            builder.HasIndex(w => w.LocationId);
            builder.HasIndex(w => w.Barcode);
            builder.HasIndex(w => new { w.Status, w.Source });
        }
    }
}

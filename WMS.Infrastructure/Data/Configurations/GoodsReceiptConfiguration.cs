using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Core.Entities;

namespace WMS.Infrastructure.Data.Configurations
{
    public class GoodsReceiptConfiguration : IEntityTypeConfiguration<GoodsReceipt>
    {
        public void Configure(EntityTypeBuilder<GoodsReceipt> builder)
        {
            builder.HasKey(entity => entity.Id);
            
            builder.HasIndex(entity => entity.GRNNumber)
                .IsUnique();
            builder.HasIndex(entity => new { entity.SupplierNip, entity.InvoiceNumber });
            builder.Property(entity => entity.GRNNumber)
                .HasMaxLength(17)
                .IsFixedLength();

            builder.Property(entity => entity.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(16)
                .IsFixedLength();

            builder.Property(entity => entity.DateOfIssue)
                .IsRequired();
            builder.Property(entity => entity.Status)
                .IsRequired()
                .HasConversion<string>();
            builder.Property(entity => entity.Comment)
                .HasMaxLength(400);

            builder.HasOne(entity => entity.Supplier)
                .WithMany()
                .HasForeignKey(entity => entity.SupplierNip)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(entity => entity.Item)
                .WithOne(entity => entity.GoodsReceipt)
                .HasForeignKey(entity => entity.GoodsReceiptID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

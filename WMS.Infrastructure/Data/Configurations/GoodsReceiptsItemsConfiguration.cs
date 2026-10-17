using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Infrastructure.Data.Configurations
{
    internal class GoodsReceiptsItemsConfiguration : IEntityTypeConfiguration<GoodsReceiptsItems>
    {
        public void Configure(EntityTypeBuilder<GoodsReceiptsItems> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.Property(entity => entity.GoodsReceiptID)
                .IsRequired();
            builder.Property(entity => entity.Barcode)
                .IsRequired()
                .HasMaxLength(13)
                .IsFixedLength();

            builder.HasIndex(entity => entity.Barcode);

            builder.Property(entity => entity.UnitQuantity)
                .IsRequired();

            builder.Property(entity => entity.Quantity)
                .HasPrecision(18, 3);
            builder.Property(entity => entity.PurchasePrice)
                .HasPrecision(18, 2);
            builder.Property(entity => entity.Status)
                .IsRequired()
                .HasConversion<string>();

        }
    }
}

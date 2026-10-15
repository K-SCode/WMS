using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Infrastructure.Data.Configurations
{
    public class GoodsReceiptConfiguration : IEntityTypeConfiguration<GoodsReceipt>
    {
        public void Configure(EntityTypeBuilder<GoodsReceipt> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.HasIndex(entity => entity.InvoiceNumber)
                .IsUnique();
            builder.HasIndex(entity => entity.GRNNumber)
                .IsUnique();
            builder.Property(entity => entity.GRNNumber)
                .HasMaxLength(17)
                .IsFixedLength();

            builder.Property(entity => entity.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(16)
                .IsFixedLength();

            builder.Property(entity => entity.DateOfIssue)
                .IsRequired();
            builder.Property(entity => entity.UserID)
                .IsRequired();
            builder.Property(entity => entity.Status)
                .IsRequired();
            builder.Property(entity => entity.Comment)
                .HasMaxLength(400);
        }
    }
}

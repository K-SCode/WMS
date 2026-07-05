using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Infrastructure.Data.Configurations
{
    public class StockConfiguration : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.HasOne(entity => entity.Localization)
                .WithMany(entity => entity.Stocks)
                .HasForeignKey(entity => entity.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(entity => entity.LocationId);

            builder.Property(entity => entity.LocationId)
                .IsRequired();

            builder.HasOne(entity => entity.Product)
                .WithMany(entity => entity.Stocks)
                .HasForeignKey(entity => entity.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(entity => entity.ProductId);

            builder.Property(entity => entity.ProductId)
                .IsRequired();

            builder.Property(entity => entity.Quantity)
                .IsRequired();
        }
    }
}

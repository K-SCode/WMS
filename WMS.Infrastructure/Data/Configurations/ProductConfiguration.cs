using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.HasIndex(entity => entity.Name);

            builder.Property(entity => entity.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(entity => entity.Price)
                .HasPrecision(18, 2)
                .IsRequired();
        }
    }
}

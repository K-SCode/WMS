using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Core.Entities;

namespace WMS.Infrastructure.Data.Configurations
{
    internal class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.CompanyName)
                .IsRequired();
            builder.Property(entity => entity.Address)
                .IsRequired();

            builder.HasIndex(entity => entity.NIP)
                .IsUnique();
            builder.Property(entity => entity.NIP)
                .HasMaxLength(10)
                .IsFixedLength();
            builder.Property(entity => entity.Phone)
                .HasMaxLength(11)
                .IsFixedLength();
        }
    }
}

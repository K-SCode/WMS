using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Core.Entities;

namespace WMS.Infrastructure.Data.Configurations
{
    internal class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.Property(entity => entity.CompanyName)
                .IsRequired();
            builder.Property(entity => entity.Address)
                .IsRequired();

            builder.HasKey(entity => entity.Nip);

            builder.Property(entity => entity.Nip)
                .HasMaxLength(10)
                .IsFixedLength();
            builder.Property(entity => entity.Phone)
                .HasMaxLength(11)
                .IsFixedLength();
        }
    }
}

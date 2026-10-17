using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Core.Entities;

namespace WMS.Infrastructure.Data.Configurations
{
    internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.FirstName)
                .HasMaxLength(100);

            builder.Property(p => p.LastName)
                .HasMaxLength(100);

            builder.Property(p => p.Phone)
                .HasMaxLength(20);

            builder.Property(p => p.Email)
                .HasMaxLength(255);

            builder.Property(p => p.Nip)
                .HasMaxLength(10)
                .IsFixedLength();

            builder.Property(p => p.CompanyName)
                .HasMaxLength(200);

            builder.HasIndex(p => p.Email)
                .IsUnique();
        }
    }
}

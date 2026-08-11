using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Core.Entities;

namespace WMS.Infrastructure.Data.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.HasIndex(entity => new { entity.Sector, entity.Rack, entity.Number })
                .IsUnique()
                .HasDatabaseName("Location_Coordinates");

            builder.Property(entity => entity.Sector)
                .IsRequired();

            builder.Property(entity => entity.Rack)
                .IsRequired();

            builder.Property(entity => entity.Number)
                .IsRequired();

            builder.Property(entity => entity.IsFull)
                .HasDefaultValue(false)
                .IsRequired();
        }
    }
}

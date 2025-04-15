using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoLocadora.Domain.Entities;

namespace MotoLocadora.Infrastructure.EntityConfiguration;

public class RiderConfiguration : IEntityTypeConfiguration<Rider>
{
    public void Configure(EntityTypeBuilder<Rider> builder)
    {
        builder.Property(e => e.LicenseDriveType)
     .HasConversion<string>();

        builder.HasIndex(e => e.CommpanyId).IsUnique();
        builder.HasIndex(e => e.LicenseDrive).IsUnique();
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.Id);
    }
}

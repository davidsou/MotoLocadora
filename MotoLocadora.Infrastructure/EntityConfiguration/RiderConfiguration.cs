using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoLocadora.Domain.Entities;

namespace MotoLocadora.Infrastructure.EntityConfiguration;

public class RiderConfiguration : IEntityTypeConfiguration<Rider>
{
    public void Configure(EntityTypeBuilder<Rider> builder)
    {
        builder.HasIndex(x => x.Id).IsUnique();
    }
}

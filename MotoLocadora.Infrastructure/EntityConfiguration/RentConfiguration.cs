using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoLocadora.Domain.Entities;

namespace MotoLocadora.Infrastructure.EntityConfiguration;

public class RentConfiguration : IEntityTypeConfiguration<Rent>
{
    public void Configure(EntityTypeBuilder<Rent> builder)
    {
        builder.HasIndex(x => x.Id).IsUnique();
    }
}

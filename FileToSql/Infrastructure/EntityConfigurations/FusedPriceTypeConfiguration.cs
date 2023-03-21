using FileToSql.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileToSql.Infrastructure.EntityConfigurations;

public class FusedPriceTypeConfiguration : IEntityTypeConfiguration<FusedPrice>
{
    public void Configure(EntityTypeBuilder<FusedPrice> builder)
    {
        builder.ToTable("FusedPrices");

        builder.HasKey(x => x.PartNumber);
    }
}

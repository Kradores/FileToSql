using FileToSql.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileToSql.Infrastructure.EntityConfigurations;

public class ListPriceTypeConfiguration : IEntityTypeConfiguration<ListPrice>
{
    public void Configure(EntityTypeBuilder<ListPrice> builder)
    {
        builder.ToTable("ListPrices");

        builder.HasKey(x => x.Id);
    }
}

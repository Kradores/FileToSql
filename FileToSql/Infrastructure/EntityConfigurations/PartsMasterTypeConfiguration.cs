using FileToSql.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileToSql.Infrastructure.EntityConfigurations;

public class PartsMasterTypeConfiguration : IEntityTypeConfiguration<PartsMaster>
{
    public void Configure(EntityTypeBuilder<PartsMaster> builder)
    {
        builder.ToTable("PartsMasters", t => t.IsMemoryOptimized());

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();
    }
}

using FileToSql.Infrastructure.Entities;
using FileToSql.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace FileToSql.Infrastructure;

public class FileManagerDbContext : DbContext
{
    public const string DEFAULT_SCHEMA = "files";

    public FileManagerDbContext(DbContextOptions<FileManagerDbContext> options) : base(options)
    {
    }

    public DbSet<PartsMaster> PartsMasters => Set<PartsMaster>();
    public DbSet<ListPrice> ListPrices => Set<ListPrice>();
    public DbSet<FusedPrice> FusedPrices => Set<FusedPrice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new PartsMasterTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ListPriceTypeConfiguration());
        modelBuilder.ApplyConfiguration(new FusedPriceTypeConfiguration());
    }
}

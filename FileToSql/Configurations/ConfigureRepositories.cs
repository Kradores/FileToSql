using FileToSql.Infrastructure.Repositories;

namespace FileToSql.Configurations;

public static class ConfigureRepositories
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddScoped<PartsMasterRepository>()
            .AddScoped<ListPriceRepository>()
            .AddScoped<FusedPriceRepository>()
            .AddScoped<UploadedFileRepository>()
            .AddScoped<FactoringRulesRepository>()
            .AddScoped<ManufacturerRepository>();
    }
}

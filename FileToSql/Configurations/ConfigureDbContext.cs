using FileToSql.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FileToSql.Configurations;

public static class ConfigureDbContext
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FileManagerDbContext>(options =>
        {
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.UseSqlServer(configuration.GetConnectionString("FileManager"),
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(FileManagerDbContext).Assembly.FullName);
                    sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                    sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", FileManagerDbContext.DEFAULT_SCHEMA);
                });
        });

        return services;
    }

    public static IApplicationBuilder MigrateAllDbContexts(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        MigrateDbContext<FileManagerDbContext>(serviceScope);

        return app;
    }

    private static void MigrateDbContext<T>(IServiceScope serviceScope) where T : DbContext
    {
        var context = serviceScope.ServiceProvider.GetRequiredService<T>();
        context.Database.Migrate();
    }
}

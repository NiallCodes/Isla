using Isla.Database.Config;
using Isla.Database.Entities;
using Isla.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Isla.Database.Installers;

public static class DatabaseInstaller
{
    /// <summary>
    /// Adds the types used by the database module to the service collection.
    /// </summary>
    public static void AddDatabaseModule(this IServiceCollection services)
    {
        // Services
        services.AddHostedService<DatabaseMigrationService>();
        services.AddDbContextFactory<DatabaseContext>((provider, builder) =>
        {
            var logger = provider.GetRequiredService<ILogger<DatabaseContext>>();
            var config = provider.GetRequiredService<DatabaseConfig>();
            logger.LogInformation("Using Postgres as the database provider");
            builder.UseNpgsql(new NpgsqlConnectionStringBuilder
            {
                Host = $"{config.Ip}:{config.Port}",
                Database = config.Database,
                Username = config.Username,
                Password = config.Password
            }.ConnectionString);
        });
    }
}
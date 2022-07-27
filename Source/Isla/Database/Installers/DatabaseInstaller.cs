using Isla.Database.Config;
using Isla.Database.Entities;
using Isla.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NiallVR.Launcher.Configuration.Binding.Extensions;
using Npgsql;

namespace Isla.Database.Installers;

public static class DatabaseInstaller
{
    /// <summary>
    /// Adds the types used by the database module to the service collection.
    /// </summary>
    public static void AddDatabaseModule(this IServiceCollection services)
    {
        // Config
        services.BindConfig<DatabaseConfig>("Database");

        // Services
        services.AddHostedService<DatabaseMigrationService>();
        services.AddDbContextFactory<DatabaseContext>((s, builder) =>
        {
            var databaseConfig = s.GetRequiredService<DatabaseConfig>();
            builder.UseNpgsql(new NpgsqlConnectionStringBuilder
            {
                Host = databaseConfig.Host,
                Database = databaseConfig.Database,
                Username = databaseConfig.Username,
                Password = databaseConfig.Password
            }.ConnectionString);
        });
    }
}
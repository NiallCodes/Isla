using Isla.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NiallVR.Launcher.Hosted.Abstract;
using Npgsql;

namespace Isla.Database.Services;

/// <summary>
/// A service which triggers the database migration upon launch.
/// </summary>
public class DatabaseMigrationService : HostedServiceBase
{
    private readonly IDbContextFactory<DatabaseContext> _dbFactory;
    private readonly ILogger<DatabaseMigrationService> _logger;

    public DatabaseMigrationService(IDbContextFactory<DatabaseContext> dbFactory, ILogger<DatabaseMigrationService> logger)
    {
        _logger = logger;
        _dbFactory = dbFactory;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting database migration");

        while (true)
        {
            try
            {
                await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);
                await db.Database.MigrateAsync(cancellationToken);
                break;
            }
            catch (NpgsqlException error)
            {
                _logger.LogError("Unable to perform migration ({Reason}), trying again in 10 seconds", error.Message);
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            }
        }

        _logger.LogInformation("Database migration complete");
    }
}
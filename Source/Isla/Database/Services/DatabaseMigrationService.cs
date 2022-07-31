using Isla.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NiallVR.Launcher.Hosted.Abstract;

namespace Isla.Database.Services;

/// <summary>
/// A service which triggers the database migration at startup.
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
        while (true)
        {
            try
            {
                _logger.LogInformation("Attempting to migrate the database");
                await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);
                await db.Database.MigrateAsync(cancellationToken);
                _logger.LogInformation("Database migrated successfully");
                return;
            }
            catch (Exception error)
            {
                _logger.LogError("Failed to migrate the database: {Reason}", error.Message);
                _logger.LogInformation("Retrying migration in 10 seconds");
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            }
        }
    }
}
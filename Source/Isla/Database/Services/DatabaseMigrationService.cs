using Isla.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NiallVR.Launcher.Hosted.Abstract;

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
        await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);
        await db.Database.MigrateAsync(cancellationToken);
        _logger.LogInformation("Database migration complete");
    }
}
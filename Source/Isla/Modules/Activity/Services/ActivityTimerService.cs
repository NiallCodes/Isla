using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Isla.Modules.Activity.Services;

public class ActivityTimerService : IHostedService
{
    private readonly DiscordSocketClient _discordClient;
    private readonly ActivityGeneratorService _activityGeneratorService;
    private readonly ILogger<ActivityTimerService> _logger;
    private readonly PeriodicTimer _periodicTimer = new(TimeSpan.FromMinutes(20));

    public ActivityTimerService(DiscordSocketClient discord, ActivityGeneratorService activityGeneratorService, ILogger<ActivityTimerService> logger)
    {
        _logger = logger;
        _discordClient = discord;
        _activityGeneratorService = activityGeneratorService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting the activity timer service");
        _ = StartTimerLoopAsync();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping the activity timer service");
        _periodicTimer.Dispose();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Periodically updates the bots displayed activity.
    /// </summary>
    private async Task StartTimerLoopAsync()
    {
        do
        {
            try
            {
                _logger.LogDebug("Updating the activity");
                await _discordClient.SetActivityAsync(_activityGeneratorService.GetNextActivity());
            }
            catch (Exception error)
            {
                _logger.LogError(error, "An exception was thrown when updating the activity");
            }
        } while (await _periodicTimer.WaitForNextTickAsync());
    }
}
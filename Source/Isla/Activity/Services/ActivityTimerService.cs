using Discord.WebSocket;
using Isla.Bootstrap.Interfaces;
using Microsoft.Extensions.Logging;

namespace Isla.Activity.Services;

/// <summary>
/// A service which periodically updates the bots activity.
/// </summary>
public class ActivityTimerService : IDisposable, IDiscordListener
{
    private readonly DiscordSocketClient _discordClient;
    private readonly ActivityService _activityService;
    private readonly ILogger<ActivityTimerService> _logger;
    private readonly PeriodicTimer _periodicTimer = new(TimeSpan.FromMinutes(20));

    public ActivityTimerService(DiscordSocketClient discord, ActivityService activityService, ILogger<ActivityTimerService> logger)
    {
        _logger = logger;
        _discordClient = discord;
        _activityService = activityService;
        
        _logger.LogInformation("Starting the Activity service");
        _ = StartTimerLoopAsync();
    }

    public void Dispose()
    {
        _logger.LogInformation("Stopping the Activity service");
        _periodicTimer.Dispose();
    }

    private async Task StartTimerLoopAsync()
    {
        do
        {
            try
            {
                _logger.LogDebug("Updating the activity");
                await _discordClient.SetActivityAsync(_activityService.GetNextActivity());
            }
            catch (Exception error)
            {
                _logger.LogError(error, "An exception was thrown when updating the activity");
            }
        } while (await _periodicTimer.WaitForNextTickAsync());
    }
}
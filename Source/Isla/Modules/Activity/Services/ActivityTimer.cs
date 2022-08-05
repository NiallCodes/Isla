using Discord.WebSocket;
using Isla.Config;
using Isla.Modules.Activity.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Isla.Modules.Activity.Services;

public class ActivityTimer : IHostedService
{
    private readonly PeriodicTimer _periodicTimer;
    private readonly IActivityGenerator _generator;
    private readonly ILogger<ActivityTimer> _logger;
    private readonly DiscordSocketClient _discordClient;

    public ActivityTimer(ActivityConfig config, DiscordSocketClient discord, IActivityGenerator generator, ILogger<ActivityTimer> logger)
    {
        _logger = logger;
        _generator = generator;
        _discordClient = discord;
        _periodicTimer = new PeriodicTimer(TimeSpan.FromMinutes(config.Frequency));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting the activity timer");
        _ = StartTimerLoopAsync();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping the activity timer");
        _periodicTimer.Dispose();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Periodically updates the activity the bot displays.
    /// </summary>
    private async Task StartTimerLoopAsync()
    {
        do
        {
            try
            {
                await _discordClient.SetActivityAsync(_generator.GetNextActivity());
            }
            catch (Exception error)
            {
                _logger.LogError(error, "An exception was thrown when updating the activity");
            }
        } while (await _periodicTimer.WaitForNextTickAsync());
    }
}
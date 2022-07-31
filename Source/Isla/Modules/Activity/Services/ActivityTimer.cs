using Discord.WebSocket;
using Isla.Config;
using Isla.Modules.Activity.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Isla.Modules.Activity.Services;

public class ActivityTimer : IHostedService
{
    private readonly ActivityConfig _config;
    private readonly DiscordSocketClient _discordClient;
    private readonly IActivityGenerator _generator;
    private readonly ILogger<ActivityTimer> _logger;
    private readonly PeriodicTimer _periodicTimer = new(TimeSpan.FromMinutes(1));

    public ActivityTimer(ActivityConfig config, DiscordSocketClient discord, IActivityGenerator generator, ILogger<ActivityTimer> logger)
    {
        _config = config;
        _logger = logger;
        _generator = generator;
        _discordClient = discord;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (_config.Enabled)
        {
            _logger.LogInformation("Enabling the Activity module");
            _ = StartTimerLoopAsync();
        } else {
            _logger.LogInformation("Disabling the Activity module");
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (_config.Enabled)
            _logger.LogInformation("Disabling the Activity module");
        
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
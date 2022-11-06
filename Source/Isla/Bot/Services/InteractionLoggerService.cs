using Discord;
using Discord.Interactions;
using Microsoft.Extensions.Logging;
using NiallCodes.Launchpad.Hosting.Utilities.Services;

namespace Isla.Bot.Services;

/// <summary>
/// A bridge between the Discord.Net and Microsoft loggers.
/// </summary>
public class InteractionLoggerService : HostedService
{
    private readonly ILogger<InteractionService> _logger;

    public InteractionLoggerService(InteractionService interaction, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<InteractionService>();
        interaction.Log += OnInteractionLog;
    }

    private Task OnInteractionLog(LogMessage log)
    {
        var logLevel = log.Severity switch
        {
            LogSeverity.Critical => LogLevel.Critical,
            LogSeverity.Error => LogLevel.Error,
            LogSeverity.Warning => LogLevel.Warning,
            LogSeverity.Info => LogLevel.Information,
            LogSeverity.Verbose => LogLevel.Trace,
            LogSeverity.Debug => LogLevel.Debug,
            _ => throw new ArgumentOutOfRangeException()
        };

        _logger.Log(logLevel, log.Exception, log.Message);
        return Task.CompletedTask;
    }
}
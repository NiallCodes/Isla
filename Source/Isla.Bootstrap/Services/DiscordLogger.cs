using System.Net.WebSockets;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using NiallVR.Launcher.Hosted.Abstract;

namespace Isla.Bootstrap.Services;

/// <summary>
/// A bridge between the Discord.Net and Microsoft loggers.
/// </summary>
internal class DiscordLogger : HostedServiceBase
{
    private readonly ILogger<DiscordSocketClient> _logger;

    public DiscordLogger(DiscordSocketClient discord, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<DiscordSocketClient>();
        discord.Log += OnDiscordLog;
    }

    private Task OnDiscordLog(LogMessage log)
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

        switch (log.Exception)
        {
            // Swap a reconnect event to be information instead of an error.
            // They're handled by Discord.Net and not usually a problem.
            case GatewayReconnectException:
                _logger.LogInformation("Server requested a reconnect");
                return Task.CompletedTask;

            // WebSocketExceptions are handled, but also logged, by Discord.Net.
            // We don't really care about them, as it's not our problem.
            case WebSocketException:
                return Task.CompletedTask;

            default:
                _logger.Log(logLevel, log.Exception, log.Message);
                return Task.CompletedTask;
        }
    }
}
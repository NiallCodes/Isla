using Discord.WebSocket;
using Isla.Bot.Interfaces;
using Isla.Modules.Notifications.Interfaces;

namespace Isla.Modules.Notifications.Listeners;

public class EventStartedListener : IDiscordListener
{
    private readonly INotificationService _notificationService;

    public EventStartedListener(DiscordSocketClient discord, INotificationService notificationService)
    {
        _notificationService = notificationService;
        discord.GuildScheduledEventStarted += HandleEventStarted;
    }

    /// <summary>
    /// Informs the activity service that an event started.
    /// </summary>
    private Task HandleEventStarted(SocketGuildEvent arg)
    {
        _notificationService.SetActivityStatus(arg.Channel.Id, true);
        return Task.CompletedTask;
    }
}
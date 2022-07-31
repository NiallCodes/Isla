using Discord.WebSocket;
using Isla.Bootstrap.Interfaces;
using Isla.Config;
using Isla.Modules.Notifications.Interfaces;

namespace Isla.Modules.Notifications.Listeners;

public class EventCancelledListener : IDiscordListener
{
    private readonly INotificationService _notificationService;

    public EventCancelledListener(NotificationConfig config, DiscordSocketClient discord, INotificationService notificationService)
    {
        _notificationService = notificationService;
        if (config.Enabled)
            discord.GuildScheduledEventCancelled += HandleEventCancelled;
    }

    /// <summary>
    /// Removes the event role from everyone.
    /// </summary>
    private async Task HandleEventCancelled(SocketGuildEvent arg)
    {
        var roleId = _notificationService.GetRoleId(arg.Channel.Id);
        if (roleId is null)
            return;

        var role = arg.Guild.GetRole(roleId.Value);
        foreach (var member in role.Members)
            await member.RemoveRoleAsync(role);
    }
}
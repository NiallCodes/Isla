using Discord.WebSocket;
using Isla.Bot.Interfaces;
using Isla.Modules.Notifications.Interfaces;

namespace Isla.Modules.Notifications.Listeners;

public class EventCancelledListener : IDiscordListener
{
    private readonly INotificationService _notificationService;

    public EventCancelledListener(DiscordSocketClient discord, INotificationService notificationService)
    {
        _notificationService = notificationService;
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
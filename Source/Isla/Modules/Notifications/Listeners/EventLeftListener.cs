using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Isla.Bot.Interfaces;
using Isla.Modules.Notifications.Interfaces;

namespace Isla.Modules.Notifications.Listeners;

public class EventLeftListener : IDiscordListener
{
    private readonly INotificationService _notificationService;

    public EventLeftListener(DiscordSocketClient discord, INotificationService notificationService)
    {
        _notificationService = notificationService;
        discord.GuildScheduledEventUserRemove += HandleUserLeftEvent;
    }

    /// <summary>
    /// Removes the event role, if the user left a configured event.
    /// </summary>
    private async Task HandleUserLeftEvent(Cacheable<SocketUser, RestUser, IUser, ulong> user, SocketGuildEvent arg)
    {
        var roleId = _notificationService.GetRoleId(arg.Channel.Id);
        if (roleId is null)
            return;

        var role = arg.Guild.GetRole(roleId.Value);
        if (role is null)
            return;

        var member = arg.Guild.GetUser(user.Id);
        if (member is null)
            return;

        await member.RemoveRoleAsync(role);
    }
}
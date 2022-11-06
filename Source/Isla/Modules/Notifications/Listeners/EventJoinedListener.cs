using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Isla.Bot.Interfaces;
using Isla.Modules.Notifications.Interfaces;

namespace Isla.Modules.Notifications.Listeners;

public class EventJoinedListener : IDiscordListener
{
    private readonly INotificationService _notificationService;

    public EventJoinedListener(DiscordSocketClient discord, INotificationService notificationService)
    {
        _notificationService = notificationService;
        discord.GuildScheduledEventUserAdd += HandleUserJoinedEvent;
    }

    /// <summary>
    /// Adds the event role, if the user joined a configured event.
    /// </summary>
    private async Task HandleUserJoinedEvent(Cacheable<SocketUser, RestUser, IUser, ulong> user, SocketGuildEvent arg)
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

        await member.AddRoleAsync(role);
    }
}
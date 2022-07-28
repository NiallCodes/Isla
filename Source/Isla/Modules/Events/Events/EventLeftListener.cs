using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Isla.Bootstrap.Interfaces;
using Isla.Modules.Events.Config;

namespace Isla.Modules.Events.Events;

public class EventLeftListener : IDiscordListener
{
    private readonly EventConfig _eventConfig;

    public EventLeftListener(EventConfig eventConfig, DiscordSocketClient discord)
    {
        _eventConfig = eventConfig;
        discord.GuildScheduledEventUserRemove += HandleUserLeftEvent;
    }

    /// <summary>
    /// Removes the event role, if the user left a configured event.
    /// </summary>
    private async Task HandleUserLeftEvent(Cacheable<SocketUser, RestUser, IUser, ulong> user, SocketGuildEvent arg)
    {
        if (_eventConfig.ChannelToRole.TryGetValue(arg.Channel.Id.ToString(), out var roleId))
            await arg.Guild.GetUser(user.Id).RemoveRoleAsync(roleId);
    }
}
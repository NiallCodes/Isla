using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Isla.Bootstrap.Interfaces;
using Isla.Modules.Events.Config;

namespace Isla.Modules.Events.Events;

public class EventJoinedListener : IDiscordListener
{
    private readonly EventConfig _eventConfig;

    public EventJoinedListener(EventConfig eventConfig, DiscordSocketClient discord)
    {
        _eventConfig = eventConfig;
        discord.GuildScheduledEventUserAdd += HandleUserJoinedEvent;
    }

    /// <summary>
    /// Adds the event role, if the user joined a configured event.
    /// </summary>
    private async Task HandleUserJoinedEvent(Cacheable<SocketUser, RestUser, IUser, ulong> user, SocketGuildEvent arg)
    {
        if (_eventConfig.ChannelToRole.TryGetValue(arg.Channel.Id.ToString(), out var roleId))
            await arg.Guild.GetUser(user.Id).AddRoleAsync(roleId);
    }
}
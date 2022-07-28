using Discord.WebSocket;
using Isla.Bootstrap.Interfaces;
using Isla.Modules.Events.Config;

namespace Isla.Modules.Events.Events;

public class EventDeletedListener : IDiscordListener
{
    private readonly EventConfig _eventConfig;

    public EventDeletedListener(EventConfig eventConfig, DiscordSocketClient discord)
    {
        _eventConfig = eventConfig;
        discord.GuildScheduledEventCancelled += HandleEventDeleted;
        discord.GuildScheduledEventCompleted += HandleEventDeleted;
    }

    /// <summary>
    /// Removes the event role for a configured event, from every user that has it.
    /// </summary>
    private async Task HandleEventDeleted(SocketGuildEvent arg)
    {
        if (!_eventConfig.ChannelToRole.TryGetValue(arg.Channel.Id.ToString(), out var roleId))
            return;

        var role = arg.Guild.GetRole(roleId);
        foreach (var member in role.Members)
            await member.RemoveRoleAsync(role);
    }
}
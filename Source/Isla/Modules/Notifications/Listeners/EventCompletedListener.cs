using Discord.WebSocket;
using Isla.Bootstrap.Interfaces;
using Isla.Config;
using Isla.Modules.Notifications.Interfaces;

namespace Isla.Modules.Notifications.Listeners;

public class EventCompletedListener : IDiscordListener
{
    private readonly INotificationService _notificationService;

    public EventCompletedListener(NotificationConfig config, DiscordSocketClient discord, INotificationService notificationService)
    {
        _notificationService = notificationService;
        if (config.Enabled)
            discord.GuildScheduledEventCompleted += HandleEventCompleted;
    }

    /// <summary>
    /// Updates the activity service and removes the role from everyone.
    /// </summary>
    private async Task HandleEventCompleted(SocketGuildEvent arg)
    {
        // Update the activity service
        _notificationService.SetActivityStatus(arg.Channel.Id, false);

        // Remove the role from everyone
        var roleId = _notificationService.GetRoleId(arg.Channel.Id);
        if (roleId is null)
            return;

        var role = arg.Guild.GetRole(roleId.Value);
        foreach (var member in role.Members)
            await member.RemoveRoleAsync(role);
    }
}
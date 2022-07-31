namespace Isla.Modules.Notifications.Models;

public class NotificationEntry
{
    /// <summary>
    /// The Channel ID the events are located in.
    /// </summary>
    public ulong? ChannelId { get; set; }

    /// <summary>
    /// The ID of the role to give to participants.
    /// </summary>
    public ulong? RoleId { get; set; }
}
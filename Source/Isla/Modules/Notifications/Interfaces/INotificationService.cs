namespace Isla.Modules.Notifications.Interfaces;

public interface INotificationService
{
    /// <summary>
    /// Returns the ID of the role to given or remove from attendees.
    /// </summary>
    /// <param name="eventChannelId">The channel the event is running in.</param>
    /// <returns>The role id to give if a matching channel is found, otherwise false.</returns>
    ulong? GetRoleId(ulong eventChannelId);

    /// <summary>
    /// Updates the activity generator for a particular event.
    /// </summary>
    /// <param name="eventChannelId">The channel the event is runing in.</param>
    /// <param name="status">True if the event is starting, false if it ended.</param>
    void SetActivityStatus(ulong eventChannelId, bool status);
}
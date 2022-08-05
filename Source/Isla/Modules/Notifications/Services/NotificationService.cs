using Isla.Config;
using Isla.Modules.Activity.Interfaces;
using Isla.Modules.Notifications.Interfaces;

namespace Isla.Modules.Notifications.Services;

public class NotificationService : INotificationService
{
    private readonly NotificationConfig _config;
    private readonly IActivityGenerator? _activityGenerator;

    public NotificationService(NotificationConfig config, IActivityGenerator? activityGenerator = null)
    {
        _config = config;
        _activityGenerator = activityGenerator;
    }

    public ulong? GetRoleId(ulong eventChannelId)
    {
        if (eventChannelId == _config.BeatSaber?.ChannelId)
            return _config.BeatSaber.RoleId;

        if (eventChannelId == _config.Dancing?.ChannelId)
            return _config.Dancing.RoleId;

        if (eventChannelId == _config.FormulaOne?.ChannelId)
            return _config.FormulaOne.RoleId;

        return null;
    }

    public void SetActivityStatus(ulong eventChannelId, bool status)
    {
        if (_activityGenerator is null)
            return;

        if (eventChannelId == _config.BeatSaber?.ChannelId)
            _activityGenerator.BeatSaberEvent = status;

        else if (eventChannelId == _config.Dancing?.ChannelId)
            _activityGenerator.DanceEvent = status;

        else if (eventChannelId == _config.FormulaOne?.ChannelId)
            _activityGenerator.FormulaOneEvent = status;
    }
}
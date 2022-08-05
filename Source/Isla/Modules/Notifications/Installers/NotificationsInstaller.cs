using Isla.Bootstrap.Extensions;
using Isla.Config;
using Isla.Modules.Global.Extensions;
using Isla.Modules.Notifications.Interfaces;
using Isla.Modules.Notifications.Listeners;
using Isla.Modules.Notifications.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Isla.Modules.Notifications.Installers;

public static class NotificationsInstaller
{
    /// <summary>
    /// Adds the types used by the event bot module to the service collection.
    /// </summary>
    public static void AddNotificationsModule(this IServiceCollection services)
    {
        if (services.IsModuleDisabled<NotificationConfig>(x => x.Enabled))
            return;

        // Event Listeners
        services.AddDiscordListener<EventCancelledListener>();
        services.AddDiscordListener<EventCompletedListener>();
        services.AddDiscordListener<EventJoinedListener>();
        services.AddDiscordListener<EventLeftListener>();
        services.AddDiscordListener<EventStartedListener>();

        // Services
        services.AddSingleton<INotificationService, NotificationService>();
    }
}
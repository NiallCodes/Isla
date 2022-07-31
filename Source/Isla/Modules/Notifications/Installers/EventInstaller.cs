using Isla.Bootstrap.Extensions;
using Isla.Modules.Notifications.Interfaces;
using Isla.Modules.Notifications.Listeners;
using Isla.Modules.Notifications.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Isla.Modules.Notifications.Installers;

public static class EventInstaller
{
    /// <summary>
    /// Adds the types used by the event bot module to the service collection.
    /// </summary>
    public static void AddEventModule(this IServiceCollection services)
    {
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
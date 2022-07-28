using Isla.Bootstrap.Extensions;
using Isla.Modules.Events.Config;
using Isla.Modules.Events.Events;
using Microsoft.Extensions.DependencyInjection;
using NiallVR.Launcher.Configuration.Binding.Extensions;

namespace Isla.Modules.Events.Installers;

public static class EventInstaller
{
    /// <summary>
    /// Adds the types used by the event bot module to the service collection.
    /// </summary>
    public static void AddEventModule(this IServiceCollection services)
    {
        // Config
        services.BindConfig<EventConfig>("Events");

        // Event Listeners
        services.AddDiscordListener<EventDeletedListener>();
        services.AddDiscordListener<EventJoinedListener>();
        services.AddDiscordListener<EventLeftListener>();
    }
}
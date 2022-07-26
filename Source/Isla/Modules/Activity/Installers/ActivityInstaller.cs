using Isla.Bootstrap.Extensions;
using Isla.Modules.Activity.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Isla.Modules.Activity.Installers;

/// <summary>
/// An extension method to add the activity module to the service collection.
/// </summary>
public static class ActivityInstaller
{
    /// <summary>
    /// Adds the services used by the activity module.
    /// </summary>
    public static void AddActivityModule(this IServiceCollection services)
    {
        // Dependencies
        services.TryAddSingleton<Random>();

        // Services
        services.AddSingleton<ActivityService>();
        services.AddDiscordListener<ActivityTimerService>();
    }
}
using Isla.Modules.Activity.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Isla.Modules.Activity.Installers;

public static class ActivityInstaller
{
    /// <summary>
    /// Adds the types used by the activity bot module to the service collection.
    /// </summary>
    public static void AddActivityModule(this IServiceCollection services)
    {
        // Dependencies
        services.TryAddSingleton<Random>();

        // Services
        services.AddSingleton<ActivityGeneratorService>();
        services.AddHostedService<ActivityTimerService>();
    }
}
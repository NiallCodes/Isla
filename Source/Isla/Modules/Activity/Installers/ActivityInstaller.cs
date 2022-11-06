using Isla.Modules.Activity.Config;
using Isla.Modules.Activity.Interfaces;
using Isla.Modules.Activity.Services;
using Isla.Modules.Global.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Isla.Modules.Activity.Installers;

public static class ActivityInstaller
{
    /// <summary>
    /// Adds the types used by the activity module to the service collection.
    /// </summary>
    public static void AddActivityModule(this IServiceCollection services)
    {
        if (services.IsModuleDisabled<ActivityConfig>(x => x.Enabled))
            return;

        // Dependencies
        services.TryAddSingleton<Random>();

        // Services
        services.AddSingleton<IActivityGenerator, ActivityGenerator>();
        services.AddHostedService<ActivityTimer>();
    }
}
using Isla.Bootstrap.Extensions;
using Isla.Config;
using Isla.Modules.Global.Extensions;
using Isla.Modules.Roles.Listeners;
using Microsoft.Extensions.DependencyInjection;

namespace Isla.Modules.Roles.Installers;

public static class RoleInstaller
{
    /// <summary>
    /// Adds the types used by the role module to the service collection.
    /// </summary>
    public static void AddRoleModule(this IServiceCollection services)
    {
        if (services.IsModuleDisabled<RoleConfig>(x => x.Enabled))
            return;

        // Event Listeners
        services.AddDiscordListener<RoleSelectMenuListener>();
        services.AddDiscordListener<RoleReadyListener>();
    }
}
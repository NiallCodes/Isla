using Isla.Bootstrap.Extensions;
using Isla.Modules.Roles.Config;
using Isla.Modules.Roles.Events;
using Microsoft.Extensions.DependencyInjection;
using NiallVR.Launcher.Configuration.Binding.Extensions;

namespace Isla.Modules.Roles.Installers;

public static class RoleInstaller
{
    /// <summary>
    /// Adds the types used by the role bot module to the service collection.
    /// </summary>
    public static void AddRoleModule(this IServiceCollection services)
    {
        // Config
        services.BindConfig<RoleConfig>("Roles");

        // Event Listeners
        services.AddDiscordListener<RoleButtonListener>();
        services.AddDiscordListener<RoleReadyListener>();
    }
}
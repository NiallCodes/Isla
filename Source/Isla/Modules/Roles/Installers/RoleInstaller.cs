using Isla.Bootstrap.Extensions;
using Isla.Modules.Roles.Listeners;
using Microsoft.Extensions.DependencyInjection;

namespace Isla.Modules.Roles.Installers;

public static class RoleInstaller
{
    /// <summary>
    /// Adds the types used by the role bot module to the service collection.
    /// </summary>
    public static void AddRoleModule(this IServiceCollection services)
    {
        // Event Listeners
        services.AddDiscordListener<RoleSelectMenuListener>();
        services.AddDiscordListener<RoleReadyListener>();
    }
}
using Isla.Client.Config;
using Microsoft.Extensions.DependencyInjection;
using NiallVR.Launcher.Configuration.Binding.Extensions;

namespace Isla.Client.Installers;

/// <summary>
/// An extension method to add the client module to the service collection.
/// </summary>
public static class ClientInstaller
{
    /// <summary>
    /// Adds the services used by the client module.
    /// </summary>
    public static void AddClientModule(this IServiceCollection services)
    {
        // Config
        services.BindConfig<DiscordClientConfig>("Discord");
    }
}
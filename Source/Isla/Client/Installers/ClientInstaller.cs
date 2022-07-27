using Isla.Client.Config;
using Microsoft.Extensions.DependencyInjection;
using NiallVR.Launcher.Configuration.Binding.Extensions;

namespace Isla.Client.Installers;

public static class ClientInstaller
{
    /// <summary>
    /// Adds the types used by the client module to the service collection.
    /// </summary>
    public static void AddClientModule(this IServiceCollection services)
    {
        // Config
        services.BindConfig<DiscordClientConfig>("Discord");
    }
}
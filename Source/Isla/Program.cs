using Discord;
using Isla.Activity.Installers;
using Isla.Bootstrap.Extensions;
using Isla.Client.Config;
using Isla.Client.Installers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NiallVR.Launcher.Configuration.Validation.Extensions;
using NiallVR.Launcher.Logging.Extensions;

await Host.CreateDefaultBuilder()
    .AddAndConfigSerilog((_, config) => { config.SetupConsoleSink(); })
    .AddConfigValidation()
    .ConfigureServices(services =>
    {
        // Modules
        services.AddClientModule();
        services.AddActivityModule();

        // Discord Configuration
        services.AddDiscord((s, settings) =>
        {
            settings.Token = s.GetRequiredService<DiscordClientConfig>().Token!;
            settings.DiscordConfig.GatewayIntents = GatewayIntents.None;
        });
    })
    .RunConsoleAsync();
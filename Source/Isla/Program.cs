using Discord;
using Isla.Bootstrap.Extensions;
using Isla.Database.Installers;
using Isla.Discord.Config;
using Isla.Discord.Installers;
using Isla.Modules.Activity.Installers;
using Isla.Modules.Roles.Installers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NiallVR.Launcher.Configuration.Validation.Extensions;
using NiallVR.Launcher.Logging.Extensions;

await Host.CreateDefaultBuilder()
    .AddAndConfigSerilog((_, config) => { config.SetupConsoleSink(); })
    .AddConfigValidation()
    .ConfigureServices(services =>
    {
        // App Modules
        services.AddClientModule();
        services.AddDatabaseModule();

        // Bot Modules
        services.AddActivityModule();
        services.AddRoleModule();

        // Discord Client Configuration
        services.AddDiscord((s, settings) =>
        {
            settings.Token = s.GetRequiredService<DiscordClientConfig>().Token!;
            settings.DiscordConfig.GatewayIntents = GatewayIntents.Guilds;
        });
    })
    .RunConsoleAsync();
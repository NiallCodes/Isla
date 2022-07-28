using Discord;
using Isla.Bootstrap.Extensions;
using Isla.Database.Installers;
using Isla.Discord.Config;
using Isla.Discord.Installers;
using Isla.Modules.Activity.Installers;
using Isla.Modules.Events.Installers;
using Isla.Modules.Roles.Installers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NiallVR.Launcher.Configuration.Validation.Extensions;
using NiallVR.Launcher.Logging.Extensions;
using Serilog.Events;

await Host.CreateDefaultBuilder()
    .AddAndConfigSerilog((_, config) =>
    {
        config.SetupConsoleSink()
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning);
    })
    .AddConfigValidation()
    .ConfigureServices(services =>
    {
        // App Modules
        services.AddClientModule();
        services.AddDatabaseModule();

        // Bot Modules
        services.AddActivityModule();
        services.AddEventModule();
        services.AddRoleModule();

        // Discord Client Configuration
        services.AddDiscord((s, settings) =>
        {
            settings.Token = s.GetRequiredService<DiscordClientConfig>().Token!;
            settings.DiscordConfig.AlwaysDownloadUsers = true;
            settings.DiscordConfig.GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMembers | GatewayIntents.GuildScheduledEvents;
        });
    })
    .RunConsoleAsync();
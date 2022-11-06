using Discord;
using Isla.Bot.Config;
using Isla.Bot.Extensions;
using Isla.Database.Installers;
using Isla.Modules.Activity.Installers;
using Isla.Modules.Notifications.Installers;
using Isla.Modules.Roles.Installers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NiallCodes.Launchpad.Configuration.Validation.Extensions;
using NiallCodes.Launchpad.Serilog.Setup.Extensions;
using Serilog;

await Host.CreateDefaultBuilder()
    .UseSerilog((_, services, config) =>
    {
        config.UseConsoleSink();
        config.UseConfiguration(services);
    })
    .ConfigureAppConfiguration((context, builder) =>
    {
        builder.AddYamlFile("config.yaml", optional: true);
        builder.AddYamlFile($"config.{context.HostingEnvironment.EnvironmentName}.yaml", optional: true);
    })
    .UseConfigValidation()
    .ConfigureServices(services =>
    {
        // Database


        // App Modules
        services.AddDatabaseModule();

        // Bot Modules
        services.AddActivityModule();
        services.AddNotificationsModule();
        services.AddRoleModule();

        // Discord Client Configuration
        services.AddDiscord((s, settings) =>
        {
            settings.Token = s.GetRequiredService<BotConfig>().Token!;
            settings.DiscordConfig.AlwaysDownloadUsers = true;
            settings.DiscordConfig.GatewayIntents =
                GatewayIntents.Guilds |
                GatewayIntents.GuildMembers |
                GatewayIntents.GuildScheduledEvents;
        });
    })
    .RunConsoleAsync();
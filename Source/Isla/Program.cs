using Discord;
using Isla.Bootstrap.Extensions;
using Isla.Config;
using Isla.Database.Installers;
using Isla.Modules.Activity.Installers;
using Isla.Modules.Notifications.Installers;
using Isla.Modules.Roles.Installers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NiallVR.Launcher.Configuration.Binding.Extensions;
using NiallVR.Launcher.Configuration.Validation.Extensions;
using NiallVR.Launcher.Logging.Extensions;
using Serilog;
using DiscordConfig = Isla.Config.DiscordConfig;

await Host.CreateDefaultBuilder()
    .ConfigureSerilog((services, config) =>
    {
        config.ReadFrom.Configuration(services.GetRequiredService<IConfiguration>());
        config.SetupConsoleSink();
    })
    .ConfigureAppConfiguration(builder =>
    {
        builder.AddYamlFile("appsettings.yaml", optional: false);
        builder.AddYamlFile("appsettings.Development.yaml", optional: true);
        builder.AddYamlFile("appsettings.Production.yaml", optional: true);
    })
    .AddConfigValidation(services =>
    {
        services.BindConfig<ActivityConfig>("Activity");
        services.BindConfig<DatabaseConfig>("Database");
        services.BindConfig<DiscordConfig>("Discord");
        services.BindConfig<NotificationConfig>("Notifications");
        services.BindConfig<RoleConfig>("Roles");
    })
    .ConfigureServices(services =>
    {
        // App Modules
        services.AddDatabaseModule();

        // Bot Modules
        services.AddActivityModule();
        services.AddEventModule();
        services.AddRoleModule();

        // Discord Client Configuration
        services.AddDiscord((s, settings) =>
        {
            settings.Token = s.GetRequiredService<DiscordConfig>().Token!;
            settings.DiscordConfig.AlwaysDownloadUsers = true;
            settings.DiscordConfig.GatewayIntents = 
                GatewayIntents.Guilds | 
                GatewayIntents.GuildMembers | 
                GatewayIntents.GuildScheduledEvents;
        });
    })
    .RunConsoleAsync();
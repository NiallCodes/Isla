using Discord.Interactions;
using Discord.WebSocket;
using Isla.Bootstrap.Interfaces;
using Isla.Bootstrap.Models;
using Isla.Bootstrap.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Isla.Bootstrap.Extensions;

public static class SetupExtensions
{
    /// <summary>
    /// Adds a Discord client to the <see cref="IHostBuilder"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the Discord client to.</param>
    /// <param name="configure">The method to use to configure the Discord client.</param>
    public static void AddDiscord(this IServiceCollection services, Action<IServiceProvider, DiscordSettings> configure)
    {
        // Settings
        services.AddSingleton(s =>
        {
            var settings = new DiscordSettings();
            configure(s, settings);
            return settings;
        });

        // Discord Client Services
        services.AddHostedService<DiscordLogger>();
        services.AddHostedService<DiscordLauncher>();
        services.AddSingleton(s =>
        {
            var discordConfig = s.GetRequiredService<DiscordSettings>().DiscordConfig;
            return new DiscordSocketClient(discordConfig);
        });

        // Interaction Services
        services.AddHostedService<InteractionHandler>();
        services.AddHostedService<InteractionLoader>();
        services.AddHostedService<InteractionLogger>();
        services.AddSingleton(s =>
        {
            var discord = s.GetRequiredService<DiscordSocketClient>();
            var interactionConfig = s.GetRequiredService<DiscordSettings>().InteractionConfig;
            return new InteractionService(discord, interactionConfig);
        });
    }

    /// <summary>
    /// Registers a <see cref="IDiscordListener"/> as its implementation type and the <see cref="IDiscordListener"/> interface. 
    /// </summary>
    /// <param name="services">The service collection to register to.</param>
    /// <typeparam name="T">The service type to register.</typeparam>
    public static void AddDiscordListener<T>(this IServiceCollection services) where T : class, IDiscordListener
    {
        services.AddSingleton<T>();
        services.AddSingleton<IDiscordListener>(s => s.GetRequiredService<T>());
    }
}
using Discord.Interactions;
using Discord.WebSocket;

namespace Isla.Bootstrap.Models;

/// <summary>
/// Configuration for the Discord client and related services.
/// </summary>
public class DiscordSettings
{
    /// <summary>
    /// The token to use when authenticating with Discord.
    /// </summary>
    public string Token { get; set; } = "";

    /// <summary>
    /// Configuration of the Discord client.
    /// </summary>
    public DiscordSocketConfig DiscordConfig { get; } = new();

    /// <summary>
    /// Configuration of the interaction service.
    /// </summary>
    public InteractionServiceConfig InteractionConfig { get; } = new();

    /// <summary>
    /// Interaction modules which should be available globally.
    /// </summary>
    internal HashSet<Type> GlobalInteractionModules { get; } = new();

    /// <summary>
    /// Interaction modules which should be available only in the guild.
    /// </summary>
    internal HashSet<Type> GuildInteractionModules { get; } = new();

    /// <summary>
    /// Adds an interaction module which will be accessible from anywhere.
    /// </summary>
    /// <typeparam name="T">The type of module to add.</typeparam>
    public void AddGlobalInteractionModule<T>() where T : InteractionModuleBase
    {
        GlobalInteractionModules.Add(typeof(T));
    }

    /// <summary>
    /// Adds an interaction module which will only be accessible from guilds.
    /// </summary>
    /// <typeparam name="T">The type of module to add.</typeparam>
    public void AddGuildInteractionModule<T>() where T : InteractionModuleBase
    {
        GuildInteractionModules.Add(typeof(T));
    }
}
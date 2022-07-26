using Discord.Interactions;
using Discord.WebSocket;
using Isla.Bootstrap.Models;
using Microsoft.Extensions.DependencyInjection;
using NiallVR.Launcher.Hosted.Abstract;

namespace Isla.Bootstrap.Services;

/// <summary>
/// A service which loads the interaction modules into the interaction service.
/// </summary>
public class InteractionLoader : HostedServiceBase
{
    private readonly DiscordSettings _settings;
    private readonly IServiceProvider _services;
    private readonly DiscordSocketClient _discord;
    private readonly InteractionService _interaction;
    private readonly List<ModuleInfo> _guildModules = new();
    private readonly List<ModuleInfo> _globalModules = new();

    public InteractionLoader(IServiceProvider services)
    {
        _services = services;
        _settings = services.GetRequiredService<DiscordSettings>();
        _discord = services.GetRequiredService<DiscordSocketClient>();
        _interaction = services.GetRequiredService<InteractionService>();
        _discord.Ready += HandleDiscordReady;
    }

    /// <summary>
    /// Adds the interaction modules to the interaction service.
    /// </summary>
    public override async Task StartAsync(CancellationToken _)
    {
        foreach (var guildModule in _settings.GuildInteractionModules)
            _guildModules.Add(await _interaction.AddModuleAsync(guildModule, _services));

        foreach (var globalModule in _settings.GlobalInteractionModules)
            _globalModules.Add(await _interaction.AddModuleAsync(globalModule, _services));
    }

    /// <summary>
    /// Registers the interaction modules globally and to the guilds.
    /// </summary>
    private async Task HandleDiscordReady()
    {
        await _interaction.AddModulesGloballyAsync(true, _globalModules.ToArray());
        foreach (var guild in _discord.Guilds)
            await _interaction.AddModulesToGuildAsync(guild, true, _guildModules.ToArray());
    }
}
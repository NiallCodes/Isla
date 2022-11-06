using Discord;
using Discord.WebSocket;
using Isla.Bot.Interfaces;
using Isla.Bot.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Isla.Bot.Services;

/// <summary>
/// A service which manages the life of the <see cref="DiscordSocketClient"/>.
/// </summary>
internal class DiscordClientService : IHostedService
{
    private readonly DiscordSettings _settings;
    private readonly DiscordSocketClient _discord;

    public DiscordClientService(DiscordSocketClient discord, DiscordSettings settings, IEnumerable<IDiscordListener> _)
    {
        _discord = discord;
        _settings = settings;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _discord.LoginAsync(TokenType.Bot, _settings.Token);
        await _discord.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _discord.StopAsync();
    }
}
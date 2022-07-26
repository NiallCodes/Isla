using Discord;
using Discord.WebSocket;
using Isla.Bootstrap.Interfaces;
using Isla.Bootstrap.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Isla.Bootstrap.Services;

/// <summary>
/// A service which manages the life of the Discord client.
/// </summary>
internal class DiscordLauncher : IHostedService
{
    private readonly IServiceProvider _services;

    public DiscordLauncher(IServiceProvider services)
    {
        _services = services;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _services.GetServices<IDiscordListener>(); // Pull them so they instantiate before Discord startup
        var settings = _services.GetRequiredService<DiscordSettings>();
        var discord = _services.GetRequiredService<DiscordSocketClient>();

        await discord.LoginAsync(TokenType.Bot, settings.Token);
        await discord.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _services.GetRequiredService<DiscordSocketClient>().StopAsync();
    }
}
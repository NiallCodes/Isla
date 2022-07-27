using System.Text;
using Discord;
using Discord.WebSocket;
using Isla.Bootstrap.Interfaces;
using Isla.Database.Entities;
using Isla.Modules.Roles.Config;
using Isla.Modules.Roles.Enums;
using Isla.Modules.Roles.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EntityFrameworkQueryableExtensions = Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions;

namespace Isla.Modules.Roles.Events;

public class RoleReadyListener : IDiscordListener
{
    private readonly RoleConfig _roleConfig;
    private readonly DiscordSocketClient _discord;
    private readonly ILogger<RoleReadyListener> _logger;
    private readonly IDbContextFactory<DatabaseContext> _dbFactory;

    public RoleReadyListener(
        RoleConfig roleConfig, DiscordSocketClient discord,
        ILogger<RoleReadyListener> logger, IDbContextFactory<DatabaseContext> dbFactory
    )
    {
        _logger = logger;
        _discord = discord;
        _dbFactory = dbFactory;
        _roleConfig = roleConfig;
        _discord.Ready += HandleReady;
    }

    private Task HandleReady()
    {
        // Start a new task to not block the gateway
        _ = UpdateRoleMessages();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Updates the role messages based on the provided config.
    /// </summary>
    private async Task UpdateRoleMessages()
    {
        _logger.LogInformation("Updating role buttons");
        await using var db = await _dbFactory.CreateDbContextAsync();
        foreach (var (roleType, category) in _roleConfig.Categories!)
        {
            try
            {
                await UpdateRoleMessage(db, roleType, category);
            }
            catch (Exception error)
            {
                _logger.LogError(error, "An unknown error occured when setting up the {RoleType} category", roleType);
            }
        }

        await db.SaveChangesAsync();
        _logger.LogInformation("Role buttons updated");
    }

    /// <summary>
    /// Updates a single role message based on the provided config.
    /// </summary>
    private async Task UpdateRoleMessage(DatabaseContext db, RoleType roleType, RoleCategory category)
    {
        // Get the database entry for the RoleType.
        var roleMessage = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(db.RoleMessages, r => r.Type == roleType);
        if (roleMessage is null)
        {
            roleMessage = new RoleMessage { Type = roleType };
            db.Add(roleMessage);
        }

        // Get the message if it exists, otherwise create a new message.
        var channel = _discord.Guilds.First().GetTextChannel(_roleConfig.ChannelId);
        if (channel is null)
        {
            _logger.LogWarning("Not updating {RoleType} category because channel {ChannelId} doesn't exist", roleType, _roleConfig.ChannelId);
            return;
        }

        // Get or create the message to update.
        var message = roleMessage.MessageId is 0 ? null : await channel.GetMessageAsync(roleMessage.MessageId);
        if (message is null)
        {
            _logger.LogInformation("Creating a new message for the {RoleType} category as one doesn't exist", roleType);
            message = await channel.SendMessageAsync("Loading");
            roleMessage.MessageId = message.Id;
        }

        // Build the message content
        var contentBuilder = new StringBuilder();
        contentBuilder.Append("__**").Append(category.Title).Append("**__\n");
        contentBuilder.Append(category.Description).Append("\n\n");
        foreach (var roleEntry in category.Roles!.Where(r => !string.IsNullOrWhiteSpace(r.Description)))
        {
            var roleName = $"{roleEntry.Emoji} {roleEntry.Title}".Trim();
            contentBuilder.Append("**").Append(roleName).Append("**\n");
            contentBuilder.Append(roleEntry.Description).Append("\n\n");
        }

        // Build the buttons
        var componentBuilder = new ComponentBuilder();
        foreach (var roleEntry in category.Roles!)
        {
            var emoji = roleEntry.Emoji is null ? null : new Emoji(roleEntry.Emoji);
            componentBuilder.WithButton(roleEntry.Title, $"role-{roleEntry.RoleId}", emote: emoji);
        }

        // Update the content of the message
        await channel.ModifyMessageAsync(message.Id, msg =>
        {
            msg.Content = contentBuilder.ToString();
            msg.Components = componentBuilder.Build();
        });
    }
}
using System.Text;
using Discord;
using Discord.WebSocket;
using Isla.Bootstrap.Interfaces;
using Isla.Database.Entities;
using Isla.Modules.Roles.Config;
using Isla.Modules.Roles.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Isla.Modules.Roles.Events;

public class RoleReadyListener : IDiscordListener
{
    private readonly RoleConfig _roleConfig;
    private readonly DiscordSocketClient _discord;
    private readonly ILogger<RoleReadyListener> _logger;
    private readonly IDbContextFactory<DatabaseContext> _dbFactory;
    private const int MessagesNeeded = 2;
    
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

    private async Task UpdateRoleMessages()
    {
        var channel = _discord.Guilds.First().GetTextChannel(_roleConfig.ChannelId);
        if (channel is null)
        {
            _logger.LogError("Not updating role buttons as the text channel doesn't exist");
            return;
        }
        
        try
        {
            await RemoveStaleMessages(channel);
            await RemoveUnneededMessages(channel);
            await CreateNewMessages(channel);
            await ClearRoleMessageContent(channel);
            await UpdateRoleMessages(channel);
        }
        catch (Exception error)
        {
            _logger.LogError(error, "Failed to update the role messages");
        }

    }

    /// <summary>
    /// Removes database entries where the message no longer exists.
    /// </summary>
    private async Task RemoveStaleMessages(IMessageChannel channel)
    {
        _logger.LogDebug("Removing stale messages from the Database");
        await using var db = await _dbFactory.CreateDbContextAsync();
        await foreach (var roleMessage in db.RoleMessages.ToAsyncEnumerable())
        {
            var message = await channel.GetMessageAsync(roleMessage.MessageId);
            if (message is null)
                db.RoleMessages.Remove(roleMessage);
        }
        await db.SaveChangesAsync();
    }

    /// <summary>
    /// Removes messages that aren't needed anymore.
    /// </summary>
    private async Task RemoveUnneededMessages(IMessageChannel channel)
    {
        _logger.LogDebug("Removing unneeded messages");
        await using var db = await _dbFactory.CreateDbContextAsync();
        var messageCount = await db.RoleMessages.CountAsync();
        await foreach (var roleMessage in db.RoleMessages.Take(messageCount).AsAsyncEnumerable())
        {
            await channel.DeleteMessageAsync(roleMessage.MessageId);
            db.RoleMessages.Remove(roleMessage);
        }
        await db.SaveChangesAsync();
    }

    /// <summary>
    /// Creates messages until the needed amount exists.
    /// </summary>
    private async Task CreateNewMessages(IMessageChannel channel)
    {
        _logger.LogDebug("Creating new messages");
        await using var db = await _dbFactory.CreateDbContextAsync();
        var amountToCreate = MessagesNeeded - await db.RoleMessages.CountAsync();
        for (var i = 0; i < Math.Max(0, amountToCreate); i++)
        {
            var message = await channel.SendMessageAsync("Loading...");
            db.Add(new RoleMessage { MessageId = message.Id, Created = message.Timestamp.ToUnixTimeSeconds()});
        }
        await db.SaveChangesAsync();
    }

    /// <summary>
    /// Changes the content of all the role messages to a loading state.
    /// </summary>
    private async Task ClearRoleMessageContent(IMessageChannel channel)
    {
        _logger.LogDebug("Clearing role message content");
        await using var db = await _dbFactory.CreateDbContextAsync();
        await foreach (var roleMessage in db.RoleMessages.ToAsyncEnumerable())
            await channel.ModifyMessageAsync(roleMessage.MessageId, msg => { msg.Content = "Loading..."; });
    }

    /// <summary>
    /// Updates the role messages with their new content.
    /// </summary>
    private async Task UpdateRoleMessages(IMessageChannel channel)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var messages = await db.RoleMessages.OrderBy(m => m.Created).ToListAsync();
        await UpdateAccessMessage(channel, messages.ElementAt(0));
        await UpdateColourMessage(channel, messages.ElementAt(1));
    }

    /// <summary>
    /// Updates the message which handles the access roles. 
    /// </summary>
    private async Task UpdateAccessMessage(IMessageChannel channel, RoleMessage roleMessage)
    {
        // Build the message content
        var contentBuilder = new StringBuilder();
        contentBuilder.Append("__**Access Roles**__\n");
        contentBuilder.Append("Roles which provide access to the various channels!").Append("\n\n");
        foreach (var roleEntry in _roleConfig.Access!)
        {
            var roleName = $"{roleEntry.Emoji} {roleEntry.Title}".Trim();
            contentBuilder.Append("**").Append(roleName).Append("**\n");
            contentBuilder.Append(roleEntry.Description).Append("\n\n");
        }
        
        // Build the buttons
        var componentBuilder = new ComponentBuilder();
        foreach (var roleEntry in _roleConfig.Access!)
        {
            var emoji = roleEntry.Emoji is null ? null : new Emoji(roleEntry.Emoji);
            componentBuilder.WithButton(roleEntry.Title, roleEntry.RoleId.ToString(), emote: emoji);
        }

        // Update the content of the message
        await channel.ModifyMessageAsync(roleMessage.MessageId, msg =>
        {
            msg.Content = contentBuilder.ToString();
            msg.Components = componentBuilder.Build();
        });
    }
    
    /// <summary>
    /// Updates the message which handles the colour roles. 
    /// </summary>
    private async Task UpdateColourMessage(IMessageChannel channel, RoleMessage roleMessage)
    {
        // Build the message content
        var contentBuilder = new StringBuilder();
        contentBuilder.Append("__**Colour Modifiers**__\n");
        contentBuilder.Append("A splash of paint for your nametag!");

        // Build the buttons
        var componentBuilder = new ComponentBuilder();
        foreach (var roleEntry in _roleConfig.Colour!)
            componentBuilder.WithButton(roleEntry.Title, roleEntry.RoleId.ToString());

        // Update the content of the message
        await channel.ModifyMessageAsync(roleMessage.MessageId, msg =>
        {
            msg.Content = contentBuilder.ToString();
            msg.Components = componentBuilder.Build();
        });
    }
}
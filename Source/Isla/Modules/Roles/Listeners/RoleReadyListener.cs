using System.Text;
using Discord;
using Discord.WebSocket;
using Isla.Bot.Interfaces;
using Isla.Database.Entities;
using Isla.Modules.Roles.Config;
using Isla.Modules.Roles.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Isla.Modules.Roles.Listeners;

public class RoleReadyListener : IDiscordListener
{
    private readonly RoleConfig _config;
    private readonly DiscordSocketClient _discord;
    private readonly ILogger<RoleReadyListener> _logger;
    private readonly IDbContextFactory<DatabaseContext> _dbFactory;

    public RoleReadyListener(RoleConfig config, DiscordSocketClient discord, ILogger<RoleReadyListener> logger, IDbContextFactory<DatabaseContext> dbFactory)
    {
        _logger = logger;
        _discord = discord;
        _dbFactory = dbFactory;
        _config = config;
        _discord.Ready += HandleReady;
    }

    private Task HandleReady()
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await RemoveStaleMessage();
                await CreateMessage();
                await UpdateMessage();
            }
            catch (Exception error)
            {
                _logger.LogError(error, "There was an error updating the role message");
            }
        });
        return Task.CompletedTask;
    }

    /// <summary>
    /// Removes the role message if it doesn't exist or is in the wrong channel.
    /// </summary>
    private async Task RemoveStaleMessage()
    {
        // Ignore if we don't have any message saved.
        await using var db = await _dbFactory.CreateDbContextAsync();
        var roleMessage = await db.RoleMessages.OrderBy(r => r.Id).FirstOrDefaultAsync();
        if (roleMessage is null)
            return;

        // Ensure the channel still exists
        var guild = _discord.Guilds.First();
        var channel = guild.GetTextChannel(roleMessage.ChannelId);
        if (channel is null)
        {
            db.RoleMessages.Remove(roleMessage);
            await db.SaveChangesAsync();
            return;
        }

        // Ensure the message still exists
        var message = await channel.GetMessageAsync(roleMessage.MessageId);
        if (message is null)
        {
            db.RoleMessages.Remove(roleMessage);
            await db.SaveChangesAsync();
            return;
        }

        // Ensure the message is in the correct channel.
        if (roleMessage.ChannelId != _config.ChannelId)
        {
            db.RoleMessages.Remove(roleMessage);
            await db.SaveChangesAsync();
            await message.DeleteAsync();
        }
    }

    /// <summary>
    /// Creates a role message if one doesn't exist.
    /// </summary>
    private async Task CreateMessage()
    {
        // Ignore if we have a message in the database.
        await using var db = await _dbFactory.CreateDbContextAsync();
        var messageCount = await db.RoleMessages.CountAsync();
        if (messageCount is not 0)
            return;

        // Send a new message.
        var guild = _discord.Guilds.First();
        var channel = guild.GetTextChannel(_config.ChannelId);
        var message = await channel.SendMessageAsync("Loading...");

        // Store the message in the DB.
        db.RoleMessages.Add(new RoleMessage { ChannelId = channel.Id, MessageId = message.Id });
        await db.SaveChangesAsync();
    }

    /// <summary>
    /// Updates the selection menu with the configured roles.
    /// </summary>
    private async Task UpdateMessage()
    {
        // Build the content
        var contentBuilder = new StringBuilder();
        contentBuilder.Append("__**Role Selector**__\n");
        contentBuilder.Append("Select the roles you want, and don't select the ones you don't want.");

        // Build the selector
        var menuBuilder = new SelectMenuBuilder();
        menuBuilder.WithPlaceholder("Select roles!").WithCustomId("role-selector");
        menuBuilder.WithMinValues(0).WithMaxValues(_config.Roles.Count);
        foreach (var roleEntry in _config.Roles)
        {
            var emoji = roleEntry.Emoji is null ? null : new Emoji(roleEntry.Emoji);
            menuBuilder.AddOption(roleEntry.Title, roleEntry.RoleId.ToString(), roleEntry.Description, emoji);
        }

        // Update the message
        await using var db = await _dbFactory.CreateDbContextAsync();
        var roleMessage = await db.RoleMessages.OrderBy(r => r.Id).FirstAsync();
        var channel = _discord.Guilds.First().GetTextChannel(roleMessage.ChannelId);
        await channel.ModifyMessageAsync(roleMessage.MessageId, msg =>
        {
            msg.Content = contentBuilder.ToString();
            msg.Components = new ComponentBuilder().WithSelectMenu(menuBuilder).Build();
        });
    }
}
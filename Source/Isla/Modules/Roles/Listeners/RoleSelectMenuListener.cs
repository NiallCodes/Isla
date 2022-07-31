using System.Text;
using Discord.WebSocket;
using Isla.Bootstrap.Interfaces;
using Isla.Config;
using Microsoft.Extensions.Logging;

namespace Isla.Modules.Roles.Listeners;

public class RoleSelectMenuListener : IDiscordListener
{
    private readonly RoleConfig _config;
    private readonly DiscordSocketClient _discord;
    private readonly ILogger<RoleSelectMenuListener> _logger;
    private readonly Dictionary<ulong, string> _roleNameLookup;
    private readonly Dictionary<ulong, SocketRole> _roleLookup = new();

    public RoleSelectMenuListener(RoleConfig config, DiscordSocketClient discord, ILogger<RoleSelectMenuListener> logger)
    {
        _config = config;
        _logger = logger;
        _discord = discord;
        _roleNameLookup = config.Roles.ToDictionary(r => r.RoleId, r => r.FriendlyName);
        if (_config.Enabled)
        {
            _discord.Ready += HandleDiscordReady;
            _discord.RoleDeleted += HandleRoleDeleted;
            _discord.SelectMenuExecuted += HandleMenuExecuted;
        }
    }

    /// <summary>
    /// Validates that all the configured roles exist.
    /// </summary>
    private Task HandleDiscordReady()
    {
        var guild = _discord.Guilds.First();
        foreach (var roleId in _roleNameLookup.Keys)
        {
            var role = guild.GetRole(roleId);
            if (role is not null)
                _roleLookup[roleId] = role;
            else
                _logger.LogWarning("Configured role does not exist in the guild: {RoleId}", roleId);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Warns when a configured role is deleted from the guild.
    /// </summary>
    private Task HandleRoleDeleted(SocketRole role)
    {
        if (!_roleLookup.ContainsKey(role.Id))
            return Task.CompletedTask;

        _roleLookup.Remove(role.Id);
        _logger.LogWarning("Configured role was deleted: {RoleId}", role.Id);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Assigns and removes roles based on the menu selection.
    /// </summary>
    private async Task HandleMenuExecuted(SocketMessageComponent arg)
    {
        // Ensure the interaction happened in the role channel.
        var happenedInRoleChannel = arg.ChannelId == _config.ChannelId;
        var customIdMatches = arg.Data.CustomId == "role-selector";
        if (!happenedInRoleChannel || !customIdMatches)
            return;

        // Since we have to make some network calls, defer to give us more time.
        await arg.DeferAsync(ephemeral: true);

        // Convert all the role id's from the interaction to ulong.
        var roleIdsToAdd = new List<ulong>(arg.Data.Values.Count);
        foreach (var selectionId in arg.Data.Values)
        {
            try
            {
                roleIdsToAdd.Add(ulong.Parse(selectionId));
            }
            catch (Exception)
            {
                _logger.LogWarning("User selected a role which isn't a valid ulong: {RoleId}", selectionId);
                await arg.RespondAsync("Sorry, something went wrong. I've informed my developer!");
                return;
            }
        }

        // Get their current role list for comparison.
        var member = (SocketGuildUser) arg.User;
        var currentRoles = member.Roles;

        // Add all of the roles
        var wantedRoles = roleIdsToAdd.Select(id => _roleLookup[id]);
        var rolesToAdd = wantedRoles.Where(r => !currentRoles.Contains(r)).ToArray();
        await member.AddRolesAsync(rolesToAdd);

        // Remove all the other roles
        var unwantedRoleIds = _roleLookup.Keys.Where(id => !roleIdsToAdd.Contains(id));
        var unwantedRoles = unwantedRoleIds.Select(id => _roleLookup[id]);
        var rolesToRemove = unwantedRoles.Where(r => currentRoles.Contains(r)).ToArray();
        await member.RemoveRolesAsync(rolesToRemove);

        // Get the names of the roles
        var addedRoleNames = rolesToAdd.Select(r => $"- {_roleNameLookup[r.Id]}").ToArray();
        var removedRoleNames = rolesToRemove.Select(r => $"- {_roleNameLookup[r.Id]}").ToArray();
        if (addedRoleNames.Length is 0 && removedRoleNames.Length is 0)
            await arg.FollowupAsync("Looks like you're already set!");
        
        // Send the response
        var stringBuilder = new StringBuilder();
        if (addedRoleNames.Length is not 0)
            stringBuilder.Append("Added:```").Append(string.Join("\n", addedRoleNames)).Append("```");
        if (removedRoleNames.Length is not 0)
            stringBuilder.Append("Removed:```").Append(string.Join("\n", removedRoleNames)).Append("```");
        await arg.FollowupAsync(stringBuilder.ToString(), ephemeral: true);
    }
}
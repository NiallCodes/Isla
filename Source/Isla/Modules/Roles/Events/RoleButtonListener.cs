using Discord.WebSocket;
using Isla.Bootstrap.Interfaces;
using Isla.Modules.Roles.Config;
using Microsoft.Extensions.Logging;

namespace Isla.Modules.Roles.Events;

public class RoleButtonListener : IDiscordListener
{
    private readonly RoleConfig _roleConfig;
    private readonly DiscordSocketClient _discord;
    private readonly ILogger<RoleButtonListener> _logger;
    private readonly IReadOnlyDictionary<ulong, string> _roleNameLookup;

    public RoleButtonListener(RoleConfig roleConfig, DiscordSocketClient discord, ILogger<RoleButtonListener> logger)
    {
        _logger = logger;
        _discord = discord;
        _roleConfig = roleConfig;
        _discord.ButtonExecuted += HandleButtonPressed;
        _roleNameLookup = roleConfig.Categories!.Values.SelectMany(s => s.Roles!).ToDictionary(e => e.RoleId, e => $"{e.Emoji} {e.Title}".Trim());
    }

    /// <summary>
    /// Assigns or removes a role upon a role button being pressed.
    /// </summary>
    private async Task HandleButtonPressed(SocketMessageComponent arg)
    {
        // Ensure the interaction happened in the role channel.
        var happenedInRoleChannel = arg.ChannelId == _roleConfig.ChannelId;
        if (!happenedInRoleChannel)
            return;

        // Convert the role id from string.
        var validRoleId = ulong.TryParse(arg.Data.CustomId[5..], out var roleId);
        if (!validRoleId)
        {
            _logger.LogWarning("Used selected role {RoleId} but it's not a valid ulong", arg.Data.CustomId[5..]);
            return;
        }

        // Get the role object from the guild.
        var selectedRole = _discord.Guilds.First().GetRole(roleId);
        if (selectedRole is null)
        {
            _logger.LogWarning("User selected role {RoleId} but the role doesn't exist", roleId);
            return;
        }

        // Pull the friendly name of the role.
        var roleName = _roleNameLookup.GetValueOrDefault(roleId);
        if (roleName is null)
        {
            _logger.LogWarning("User {UserId} selected role {RoleId} but it's not configured", arg.User.Id, roleId);
            return;
        }

        // Assign or revoke the role.
        var member = (SocketGuildUser) arg.User;
        var memberHasRole = member.Roles.Contains(selectedRole);
        if (memberHasRole)
        {
            await member.RemoveRoleAsync(selectedRole);
            await arg.RespondAsync($"Removed the `{roleName}` role!", ephemeral: true);
        }
        else
        {
            await member.AddRoleAsync(selectedRole);
            await arg.RespondAsync($"Assigned the `{roleName}` role!", ephemeral: true);
        }
    }
}
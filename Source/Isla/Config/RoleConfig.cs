using Isla.Modules.Roles.Models;

namespace Isla.Config;

public class RoleConfig
{
    /// <summary>
    /// True if the Role module should be enabled, false if not.
    /// Default: True
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Channel ID of where role messages should be located.
    /// </summary>
    public ulong ChannelId { get; set; }

    /// <summary>
    /// The collection of roles to add to the select menu.
    /// </summary>
    public List<RoleEntry> Roles { get; set; } = new();
}
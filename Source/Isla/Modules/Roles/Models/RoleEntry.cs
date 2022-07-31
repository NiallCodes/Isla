namespace Isla.Modules.Roles.Models;

public class RoleEntry
{
    /// <summary>
    /// The Discord ID of the role that should be given/removed.
    /// </summary>
    public ulong RoleId { get; set; }

    /// <summary>
    /// The title of the role, displayed on the button.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The emoji used to represent the role on the button.
    /// </summary>
    public string? Emoji { get; set; }

    /// <summary>
    /// A short description about the role.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// The UI friendly name of the role.
    /// </summary>
    public string FriendlyName => $"{Emoji} {Title}".Trim();
}
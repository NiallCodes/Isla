using System.ComponentModel.DataAnnotations;
using NiallVR.Launcher.Configuration.Validation.Abstract;
using NiallVR.Launcher.Configuration.Validation.Interfaces;

namespace Isla.Modules.Roles.Config;

public class RoleConfig : IValidatedConfig
{
    /// <summary>
    /// The Discord ID of the channel where role messages should be placed.
    /// </summary>
    [Required]
    public ulong ChannelId { get; set; }

    /// <summary>
    /// The collection of channel access roles.
    /// </summary>
    [Required]
    public List<RoleEntry>? Access { get; set; }

    /// <summary>
    /// The collection of colour roles.
    /// </summary>
    [Required]
    public List<RoleEntry>? Colour { get; set; }

    /// <summary>
    /// Validates the role config and the nested categories.
    /// </summary>
    public void ValidateConfig()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
        foreach (var accessRoles in Access!)
            accessRoles.ValidateConfig();
        foreach (var colourRoles in Colour!)
            colourRoles.ValidateConfig();
    }
}

public class RoleEntry : ValidatedConfig
{
    /// <summary>
    /// The Discord ID of the role that should be given/removed.
    /// </summary>
    [Required]
    public ulong RoleId { get; set; }

    /// <summary>
    /// The emoji used to represent the role on the button.
    /// </summary>
    public string? Emoji { get; set; }

    /// <summary>
    /// The title of the role, displayed on the button.
    /// </summary>
    [Required]
    public string? Title { get; set; }

    /// <summary>
    /// A short description about the role.
    /// </summary>
    public string? Description { get; set; }
}
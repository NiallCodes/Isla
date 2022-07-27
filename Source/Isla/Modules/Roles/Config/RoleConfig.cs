using System.ComponentModel.DataAnnotations;
using Isla.Modules.Roles.Enums;
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
    /// The various role messages.
    /// </summary>
    [Required]
    public IDictionary<RoleType, RoleCategory>? Categories { get; set; }

    /// <summary>
    /// Validates the role config and the nested categories.
    /// </summary>
    public void ValidateConfig()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
        foreach (var roleCategory in Categories!.Values)
            roleCategory.ValidateConfig();
    }
}

public class RoleCategory : IValidatedConfig
{
    /// <summary>
    /// The title of the role category message.
    /// </summary>
    [Required]
    public string? Title { get; set; }

    /// <summary>
    /// The description located under the title.
    /// </summary>
    [Required]
    public string? Description { get; set; }

    /// <summary>
    /// The list of role buttons.
    /// </summary>
    [Required]
    public IReadOnlyCollection<RoleEntry>? Roles { get; set; }

    /// <summary>
    /// Validates the category and the nested roles.
    /// </summary>
    public void ValidateConfig()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
        foreach (var roleEntry in Roles!)
            roleEntry.ValidateConfig();
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
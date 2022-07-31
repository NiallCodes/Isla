using System.ComponentModel.DataAnnotations;
using NiallVR.Launcher.Configuration.Validation.Abstract;

namespace Isla.Config;

public class DiscordConfig : ValidatedConfig
{
    /// <summary>
    /// The token used to authenticate as the bot user.
    /// </summary>
    /// <remarks>Required</remarks>
    [Required]
    public string? Token { get; set; }
}
using System.ComponentModel.DataAnnotations;
using NiallVR.Launcher.Configuration.Validation.Abstract;

namespace Isla.Client.Config;

public class DiscordClientConfig : ValidatedConfig
{
    /// <summary>
    /// The token used to authenticate as the bot user.
    /// </summary>
    [Required]
    public string? Token { get; set; }
}
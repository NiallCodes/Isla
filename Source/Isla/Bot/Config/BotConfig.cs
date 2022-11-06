using System.ComponentModel.DataAnnotations;
using NiallCodes.Launchpad.Configuration.Validation.Models;

namespace Isla.Bot.Config;

public class BotConfig : ValidatedConfig
{
    /// <summary>
    /// The token used to authenticate as the bot user.
    /// </summary>
    /// <remarks>Required</remarks>
    [Required]
    public string? Token { get; set; }
}
using System.ComponentModel.DataAnnotations;
using NiallVR.Launcher.Configuration.Validation.Abstract;

namespace Isla.Modules.Events.Config;

public class EventConfig : ValidatedConfig
{
    /// <summary>
    /// The role that should be given/removed upon joining/leaving an event respectively.
    /// </summary>
    [Required]
    public Dictionary<string, ulong> ChannelToRole { get; set; }
}
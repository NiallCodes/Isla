using Isla.Modules.Notifications.Models;
using NiallVR.Launcher.Configuration.Validation.Abstract;

namespace Isla.Config;

public class NotificationConfig : ValidatedConfig
{
    /// <summary>
    /// True if the Notification module should be enabled, false if not.
    /// Default: True
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// The configuration for Beat Saber session events.
    /// </summary>
    public NotificationEntry? BeatSaber { get; set; }

    /// <summary>
    /// The configuration for Dancing session events.
    /// </summary>
    public NotificationEntry? Dancing { get; set; }

    /// <summary>
    /// The configuration for F1 session events.
    /// </summary>
    public NotificationEntry? FormulaOne { get; set; }
}
using Discord;

namespace Isla.Modules.Activity.Constants;

public static class ActivityConstants
{
    /// <summary>
    /// The activity displayed when a Beat Saber session is active.
    /// </summary>
    public static Game BeatSaber { get; } = new("Beat Saber", ActivityType.Playing);

    /// <summary>
    /// The activity displayed when a dance session is active.
    /// </summary>
    public static Game Dance { get; } = new("People dancing", ActivityType.Watching);

    /// <summary>
    /// The activity displayed when an F1 session is active.
    /// </summary>
    public static Game FormulaOne { get; } = new("F1", ActivityType.Watching);

    /// <summary>
    /// The activities that should be displayed when no events are happening.
    /// </summary>
    public static Game[] GenericActivities { get; } =
    {
        // Listening
        new("to Spotify", ActivityType.Listening),

        // Playing
        new("SAO (Beta)", ActivityType.Playing),
        new("Minecraft", ActivityType.Playing),
        new("VRChat", ActivityType.Playing),

        // Watching
        new("Anime", ActivityType.Watching),
        new("Niall make Syntax errors", ActivityType.Watching),
        new("Twitch", ActivityType.Watching),
        new("YouTube", ActivityType.Watching),
        new("You", ActivityType.Watching),
    };
}
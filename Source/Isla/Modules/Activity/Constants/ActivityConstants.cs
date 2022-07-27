using Discord;

namespace Isla.Modules.Activity.Constants;

public static class ActivityConstants
{
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
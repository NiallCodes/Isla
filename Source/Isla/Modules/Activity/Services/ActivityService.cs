using Discord;
using Isla.Modules.Activity.Constants;

namespace Isla.Modules.Activity.Services;

/// <summary>
/// A service which selects an activity based on the internal state.
/// </summary>
public class ActivityService
{
    private readonly Random _random;

    public ActivityService(Random random)
    {
        _random = random;
    }

    /// <summary>
    /// Generates the activity to display based on the internal state.
    /// </summary>
    /// <returns>The activity the bot should broadcast.</returns>
    public Game GetNextActivity()
    {
        return ActivityConstants.GenericActivities[_random.Next(ActivityConstants.GenericActivities.Length)];
    }
}
using Discord;
using Isla.Modules.Activity.Constants;

namespace Isla.Modules.Activity.Services;

public class ActivityGeneratorService
{
    private readonly Random _random;

    public ActivityGeneratorService(Random random)
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
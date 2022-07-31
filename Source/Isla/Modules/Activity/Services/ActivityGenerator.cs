using Discord;
using Isla.Modules.Activity.Constants;
using Isla.Modules.Activity.Interfaces;

namespace Isla.Modules.Activity.Services;

public class ActivityGenerator : IActivityGenerator
{
    public bool BeatSaberEvent { get; set; }
    public bool DanceEvent { get; set; }
    public bool FormulaOneEvent { get; set; }

    private readonly Random _random;

    public ActivityGenerator(Random random)
    {
        _random = random;
    }

    public Game GetNextActivity()
    {
        if (BeatSaberEvent)
            return ActivityConstants.BeatSaber;

        if (DanceEvent)
            return ActivityConstants.Dance;

        if (FormulaOneEvent)
            return ActivityConstants.FormulaOne;

        return ActivityConstants.GenericActivities[_random.Next(ActivityConstants.GenericActivities.Length)];
    }
}
using Discord;

namespace Isla.Modules.Activity.Interfaces;

public interface IActivityGenerator
{
    /// <summary>
    /// True if a Beat Saber event is active, otherwise false.
    /// </summary>
    bool BeatSaberEvent { set; }

    /// <summary>
    /// True if a dance session is active, otherwise false.
    /// </summary>
    bool DanceEvent { set; }

    /// <summary>
    /// True if an F1 session is active, otherwise false.
    /// </summary>
    bool FormulaOneEvent { set; }

    /// <summary>
    /// Generates the activity to display based on the internal state.
    /// </summary>
    /// <returns>The activity the bot should broadcast.</returns>
    Game GetNextActivity();
}
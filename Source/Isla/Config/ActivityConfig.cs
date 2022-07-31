namespace Isla.Config;

public class ActivityConfig
{
    /// <summary>
    /// True if the Activity module should be enabled, false if not.
    /// Default: True
    /// </summary>
    public bool Enabled { get; set; } = true;
    
    /// <summary>
    /// How many minutes between activity updates.
    /// Default: 5 Minutes
    /// </summary>
    public int Frequency { get; set; } = 5;
}
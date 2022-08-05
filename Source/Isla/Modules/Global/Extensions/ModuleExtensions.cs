using Microsoft.Extensions.DependencyInjection;

namespace Isla.Modules.Global.Extensions;

public static class ModuleExtensions
{
    /// <summary>
    /// Returns if the module is disabled or not.
    /// </summary>
    /// <param name="service">The service collection to pull the config from.</param>
    /// <param name="enabledFunc">The function to determine if the module is enabled or not.</param>
    /// <typeparam name="T">The config for the module.</typeparam>
    /// <returns>True if the module is disabled, otherwise false.</returns>
    public static bool IsModuleDisabled<T>(this IServiceCollection service, Func<T, bool> enabledFunc) where T : notnull
    {
        var provider = service.BuildServiceProvider();
        var config = provider.GetRequiredService<T>();
        return !enabledFunc(config);
    }
}
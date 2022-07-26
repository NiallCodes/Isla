using System.ComponentModel.DataAnnotations;
using NiallVR.Launcher.Configuration.Validation.Abstract;

namespace Isla.Database.Config;

/// <summary>
/// The configuration model for the database module.
/// </summary>
public class DatabaseConfig : ValidatedConfig
{
    /// <summary>
    /// The IP:Port of the postgres server.
    /// </summary>
    [Required]
    public string? Host { get; set; }
    
    /// <summary>
    /// The name of the database the application should use.
    /// </summary>
    [Required]
    public string? Database { get; set; }
    
    /// <summary>
    /// The username to use when authenticating with the server.
    /// </summary>
    [Required]
    public string? Username { get; set; }
    
    /// <summary>
    /// The password to use when authenticating with the server.
    /// </summary>
    [Required]
    public string? Password { get; set; }
}
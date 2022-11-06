using NiallCodes.Launchpad.Configuration.Validation.Models;

namespace Isla.Database.Config;

public class DatabaseConfig : ValidatedConfig
{
    /// <summary>
    /// The IP address of the server Postgres is running on.
    /// Default: 127.0.0.1
    /// </summary>
    public string Ip { get; set; } = "127.0.0.1";

    /// <summary>
    /// The port postgres is running on.
    /// Default: 5432
    /// </summary>
    public int Port { get; set; } = 5432;

    /// <summary>
    /// The name of the database on the Postgres server.
    /// Default: Isla
    /// </summary>
    public string Database { get; set; } = "Isla";

    /// <summary>
    /// The username to use when authenticating with the Postgres server.
    /// Default: postgres
    /// </summary>
    public string Username { get; set; } = "postgres";

    /// <summary>
    /// The password to use when authenticating with the Postgres server.
    /// </summary>
    public string? Password { get; set; }
}
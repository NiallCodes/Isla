using System.ComponentModel.DataAnnotations;

namespace Isla.Modules.Roles.Models;

/// <summary>
/// The persistant id of a message holding role information.
/// </summary>
public class RoleMessage
{
    /// <summary>
    /// The unique identifier of this database entry.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The UTC timestamp of when the message was created.
    /// </summary>
    public long Created { get; set; }
    
    /// <summary>
    /// The Discord ID of the message.
    /// </summary>
    public ulong MessageId { get; set; }
}
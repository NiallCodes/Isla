using System.ComponentModel.DataAnnotations;
using Isla.Modules.Roles.Enums;
using Microsoft.EntityFrameworkCore;

namespace Isla.Modules.Roles.Models;

/// <summary>
/// Persisted information about a role message.
/// </summary>
[Index(nameof(Type), IsUnique = true)]
public class RoleMessage
{
    /// <summary>
    /// The unique identifier of this database entry.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The type of role the message is for.
    /// </summary>
    public RoleType Type { get; set; }

    /// <summary>
    /// The Discord ID of the message in the role channel.
    /// </summary>
    public ulong MessageId { get; set; }
}
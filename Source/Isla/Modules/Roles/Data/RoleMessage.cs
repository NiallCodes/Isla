using System.ComponentModel.DataAnnotations;

namespace Isla.Modules.Roles.Data;

public class RoleMessage
{
    /// <summary>
    /// The unique identifier of this database entry.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The ID of the channel the message was posted in.
    /// </summary>
    public ulong ChannelId { get; set; }

    /// <summary>
    /// The ID of the message.
    /// </summary>
    public ulong MessageId { get; set; }
}
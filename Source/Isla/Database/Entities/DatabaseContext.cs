using Isla.Modules.Roles.Models;
using Microsoft.EntityFrameworkCore;

namespace Isla.Database.Entities;

public class DatabaseContext : DbContext
{
    /// <summary>
    /// Database entries for the role messages.
    /// </summary>
    public DbSet<RoleMessage> RoleMessages => Set<RoleMessage>();

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
}
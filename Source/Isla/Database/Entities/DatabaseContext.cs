using Isla.Modules.Roles.Data;
using Microsoft.EntityFrameworkCore;

namespace Isla.Database.Entities;

public class DatabaseContext : DbContext
{
    /// <summary>
    /// Role messages used in the Roles module.
    /// </summary>
    public DbSet<RoleMessage> RoleMessages => Set<RoleMessage>();

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
}
using Isla.Modules.Roles.Models;
using Microsoft.EntityFrameworkCore;

namespace Isla.Database.Entities;

public class DatabaseContext : DbContext
{
    public DbSet<RoleMessage> RoleMessages => Set<RoleMessage>();

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
}
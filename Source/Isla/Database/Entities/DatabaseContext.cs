using Microsoft.EntityFrameworkCore;

namespace Isla.Database.Entities;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
}
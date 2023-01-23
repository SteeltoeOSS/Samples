using Microsoft.EntityFrameworkCore;
using MySqlEFCore.Entities;

namespace MySqlEFCore.Data;

internal sealed class OtherDbContext : DbContext
{
    public DbSet<OtherEntity> OtherEntities => Set<OtherEntity>();

    public OtherDbContext(DbContextOptions<OtherDbContext> options)
        : base(options)
    {
    }
}

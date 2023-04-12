using Microsoft.EntityFrameworkCore;
using MySqlEFCore.Entities;

namespace MySqlEFCore.Data;

public sealed class AppDbContext : DbContext
{
    public DbSet<SampleEntity> SampleEntities => Set<SampleEntity>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}

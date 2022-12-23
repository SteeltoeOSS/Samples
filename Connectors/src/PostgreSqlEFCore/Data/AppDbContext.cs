using Microsoft.EntityFrameworkCore;
using PostgreSqlEFCore.Entities;

namespace PostgreSqlEFCore.Data;

public sealed class AppDbContext : DbContext
{
    public DbSet<SampleEntity> SampleEntities => Set<SampleEntity>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}

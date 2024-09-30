using Microsoft.EntityFrameworkCore;
using Steeltoe.Samples.MySqlEFCore.Entities;

namespace Steeltoe.Samples.MySqlEFCore.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<SampleEntity> SampleEntities => Set<SampleEntity>();
}

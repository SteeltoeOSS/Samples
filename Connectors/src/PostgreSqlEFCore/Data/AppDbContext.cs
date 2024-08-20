using Microsoft.EntityFrameworkCore;
using Steeltoe.Samples.PostgreSqlEFCore.Entities;

namespace Steeltoe.Samples.PostgreSqlEFCore.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<SampleEntity> SampleEntities => Set<SampleEntity>();
}

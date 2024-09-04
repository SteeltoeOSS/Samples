using Microsoft.EntityFrameworkCore;
using Steeltoe.Samples.SqlServerEFCore.Entities;

namespace Steeltoe.Samples.SqlServerEFCore.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<SampleEntity> SampleEntities => Set<SampleEntity>();
}

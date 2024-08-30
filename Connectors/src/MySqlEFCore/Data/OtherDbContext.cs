using Microsoft.EntityFrameworkCore;
using Steeltoe.Samples.MySqlEFCore.Entities;

namespace Steeltoe.Samples.MySqlEFCore.Data;

internal sealed class OtherDbContext(DbContextOptions<OtherDbContext> options) : DbContext(options)
{
    public DbSet<OtherEntity> OtherEntities => Set<OtherEntity>();
}

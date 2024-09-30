using Microsoft.EntityFrameworkCore;
using Steeltoe.Samples.FortuneTellerApi.Models;

namespace Steeltoe.Samples.FortuneTellerApi.Data;

public sealed class FortuneDbContext(DbContextOptions<FortuneDbContext> options) : DbContext(options)
{
    public DbSet<Fortune> Fortunes => Set<Fortune>();
}

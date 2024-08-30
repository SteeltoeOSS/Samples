using Microsoft.EntityFrameworkCore;
using Steeltoe.Samples.FortuneTellerService.Models;

namespace Steeltoe.Samples.FortuneTellerService.Data;

public sealed class FortuneDbContext(DbContextOptions<FortuneDbContext> options) : DbContext(options)
{
    public DbSet<Fortune> Fortunes => Set<Fortune>();
}

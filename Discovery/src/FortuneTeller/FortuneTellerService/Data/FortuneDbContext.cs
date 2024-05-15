using FortuneTellerService.Models;
using Microsoft.EntityFrameworkCore;

namespace FortuneTellerService.Data;

public sealed class FortuneDbContext(DbContextOptions<FortuneDbContext> options) : DbContext(options)
{
    public DbSet<Fortune> Fortunes => Set<Fortune>();
}

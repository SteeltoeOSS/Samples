using Microsoft.EntityFrameworkCore;

namespace FortuneTellerService.Models
{
    public class FortuneContext : DbContext
    {
        public FortuneContext(DbContextOptions<FortuneContext> options) :
            base(options)
        {

        }
        public DbSet<Fortune> Fortunes { get; set; }
    }
}
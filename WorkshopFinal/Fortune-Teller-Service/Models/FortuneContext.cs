using Microsoft.EntityFrameworkCore;


namespace Fortune_Teller_Service.Models
{
    public class FortuneContext : DbContext
    {
        public FortuneContext(DbContextOptions<FortuneContext> options) :
            base(options)
        {

        }
        public DbSet<FortuneEntity> Fortunes { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;

namespace MusicStore.Models
{

    public class MusicStoreContext : DbContext
    {
        public MusicStoreContext(DbContextOptions options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Album>().Property(b => b.Price).HasColumnType("decimal(4,2)");
        }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }

    }


}

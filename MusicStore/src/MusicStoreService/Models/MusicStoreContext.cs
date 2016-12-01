using Microsoft.EntityFrameworkCore;

namespace MusicStore.Models
{

    public class MusicStoreContext : DbContext
    {
        public MusicStoreContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }

    }


}

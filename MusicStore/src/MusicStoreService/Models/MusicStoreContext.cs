using MySql.Data.Entity;
using System.Data.Entity;

namespace MusicStore.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]

    public class MusicStoreContext : DbContext
    {
        public MusicStoreContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }

    }
}

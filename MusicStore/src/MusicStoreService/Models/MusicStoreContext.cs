
#if NET451 && MYSQL
using MySql.Data.Entity;
using System.Data.Entity;
#endif

#if !NET451 || POSTGRES
using Microsoft.EntityFrameworkCore;
#endif


namespace MusicStore.Models
{
#if NET451 && MYSQL
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
#endif


#if !NET451 || POSTGRES
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
#endif

}

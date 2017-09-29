using System.Data.Entity;

namespace MsSql4.Data
{
    public class BloggingContext : DbContext
    {
        public BloggingContext()
        {
            Database.SetInitializer(new BloggingContextInitializer());
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}
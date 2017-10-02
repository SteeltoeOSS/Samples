using System.Collections.Generic;
using System.Data.Entity;

namespace MsSql4.Data
{
    public class BloggingContext : DbContext, IBloggingContext
    {
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        public BloggingContext()
        {
            Database.SetInitializer(new BloggingContextInitializer());
        }

        public string GetDatasource()
        {
            return Database.Connection.DataSource;
        }

        public string GetDatabase()
        {
            return Database.Connection.Database;
        }

        public IEnumerable<Blog> GetBlogs ()
        {
            return Blogs.Include(p => p.Posts);
        }

        public IEnumerable<Post> GetPosts ()
        {
            return Posts;
        }
    }
}
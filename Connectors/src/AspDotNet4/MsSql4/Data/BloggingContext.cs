using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;

namespace MsSql4.Data
{
    public class BloggingContext : DbContext, IBloggingContext
    {
        public BloggingContext(DbConnection connection) : base(connection, false)
        {
            Console.Out.WriteLine("Setting Context Initializer");
            Database.SetInitializer(new BloggingContextInitializer());
            Console.Out.WriteLine("Context Initializer Set");
        }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        /// <summary>
        /// don't do this in real life
        /// </summary>
        /// <returns></returns>
        public string GetFullConnectionString()
        {
#if DEBUG
            return Database.Connection.ConnectionString;
#else
            return "Connection string will only be returned in debug mode";
#endif
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
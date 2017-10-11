using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace MsSql4.Data
{
    public class BloggingContextInitializer : CreateDatabaseIfNotExists<BloggingContext>
    {
        protected override void Seed(BloggingContext context)
        {
            Console.Out.WriteLine("Seeding Database");
            try
            {
                var blog = new Blog { Name = "Sample" };
                var posts = new List<Post> {
                    new Post { Title = "First Post", Content = "This is the first sample post" },
                    new Post { Title = "Second Post", Content = "This is the second sample post" }
                };
                blog.Posts = posts;
                context.Blogs.Add(blog);
                context.SaveChanges();
            }
            catch(Exception e)
            {
                Console.Error.WriteLine(e.ToString());
            }

            base.Seed(context);
        }
    }
}
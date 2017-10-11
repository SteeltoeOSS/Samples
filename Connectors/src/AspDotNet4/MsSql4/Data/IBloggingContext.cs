using System.Collections.Generic;

namespace MsSql4.Data
{
    public interface IBloggingContext
    {
        string GetDatasource();

        string GetDatabase();

        /// <summary>
        /// don't do this in real life
        /// </summary>
        /// <returns></returns>
        string GetFullConnectionString();

        IEnumerable<Blog> GetBlogs();

        IEnumerable<Post> GetPosts();
    }
}
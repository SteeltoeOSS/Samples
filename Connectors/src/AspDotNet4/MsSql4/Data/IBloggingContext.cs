using System.Collections.Generic;

namespace MsSql4.Data
{
    public interface IBloggingContext
    {
        string GetDatasource();

        string GetDatabase();

        IEnumerable<Blog> GetBlogs();

        IEnumerable<Post> GetPosts();
    }
}
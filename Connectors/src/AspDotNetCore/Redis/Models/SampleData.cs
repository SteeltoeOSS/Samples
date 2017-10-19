using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Redis.Models
{
    public class SampleData
    {
        internal static async Task InitializeCache(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var cache = serviceScope.ServiceProvider.GetService<IDistributedCache>();
                if (cache != null)
                {

                    await cache.SetAsync("Key1", Encoding.UTF8.GetBytes("Key1Value"));
                    await cache.SetAsync("Key2", Encoding.UTF8.GetBytes("Key2Value"));
                }
                var conn = serviceScope.ServiceProvider.GetService<IConnectionMultiplexer>();
                if (conn != null)
                {
                    var db = conn.GetDatabase();
                    db.StringSet("ConnectionMultiplexerKey1", "Key1Value via ConnectionMultiplexer");
                    db.StringSet("ConnectionMultiplexerKey2", "Key2Value via ConnectionMultiplexer");
                }
            }
        }
    }
}

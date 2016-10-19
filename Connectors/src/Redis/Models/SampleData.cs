using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;

using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

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

                    await cache.SetAsync("Key1", Encoding.Default.GetBytes("Key1Value"));
                    await cache.SetAsync("Key2", Encoding.Default.GetBytes("Key2Value"));
                }
                var conn = serviceScope.ServiceProvider.GetService<ConnectionMultiplexer>();
                if (conn != null)
                {
                    var db = conn.GetDatabase();
                    db.StringSet("Key1", "Key1Value via ConnectionMultiplexor");
                    db.StringSet("Key2", "Key2Value via ConnectionMultiplexor");
                }
            }
        }
    }
}

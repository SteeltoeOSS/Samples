using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pivotal.Extensions.Caching;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;

namespace RedisCloudFoundry.Models
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
            }
        }
    }
}

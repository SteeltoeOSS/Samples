using Autofac;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using StackExchange.Redis;
using System;
using System.Text;

namespace Redis4.Models
{
    public class SampleData
    {
        internal static void InitializeCache(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            using (var connection = container.Resolve<RedisCache>())
            {
                connection.Set("RedisCacheKey1", Encoding.UTF8.GetBytes("Key1Value via RedisCache"));
                connection.Set("RedisCacheKey2", Encoding.UTF8.GetBytes("Key2Value via RedisCache"));
            }

            using (var connection = container.Resolve<ConnectionMultiplexer>())
            {
                var db = connection.GetDatabase();
                db.StringSet("ConnectionMultiplexerKey1", "Key1Value via ConnectionMultiplexer");
                db.StringSet("ConnectionMultiplexerKey2", "Key2Value via ConnectionMultiplexer");
            }
        }
    }
}
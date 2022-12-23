using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Redis;

internal sealed class RedisSeeder
{
    public static async Task CreateSampleDataAsync(IServiceProvider serviceProvider)
    {
        await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();

        try
        {
            var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();

            await cache.SetAsync("Key1", Encoding.UTF8.GetBytes("CacheValue1"));
            await cache.SetAsync("Key2", Encoding.UTF8.GetBytes("CacheValue2"));
        }
        catch (RedisException exception)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<RedisSeeder>>();
            logger.LogError(exception, "Failed to create sample data via IDistributedCache.");
        }

        try
        {
            var multiplexer = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();

            IDatabase database = multiplexer.GetDatabase();
            database.StringSet("ConnectionMultiplexerKey1", "ConnectionMultiplexerValue1");
            database.StringSet("ConnectionMultiplexerKey2", "ConnectionMultiplexerValue2");
        }
        catch (Exception exception) when (exception.InnerException is RedisException)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<RedisSeeder>>();
            logger.LogError(exception, "Failed to create sample data via IConnectionMultiplexer.");
        }
    }
}

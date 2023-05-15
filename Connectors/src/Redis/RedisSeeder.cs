using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using Steeltoe.Connectors;
using Steeltoe.Connectors.Redis;

namespace Redis;

internal sealed class RedisSeeder
{
    private static readonly TimeSpan ExampleSlidingExpiration = TimeSpan.FromSeconds(30);

    public static async Task CreateSampleDataAsync(IServiceProvider serviceProvider)
    {
        await CreateSampleDataUsingDistributedCacheAsync(serviceProvider);
        await CreateSampleDataUsingConnectionMultiplexerAsync(serviceProvider);
    }

    private static async Task CreateSampleDataUsingDistributedCacheAsync(IServiceProvider serviceProvider)
    {
        var connectorFactory = serviceProvider.GetRequiredService<ConnectorFactory<RedisOptions, IDistributedCache>>();
        Connector<RedisOptions, IDistributedCache> connector = connectorFactory.GetDefault();
        IDistributedCache distributedCache = connector.GetConnection();

        var entryOptions = new DistributedCacheEntryOptions
        {
            SlidingExpiration = ExampleSlidingExpiration
        };

        await distributedCache.SetStringAsync("KeySetUsingMicrosoftApi1", "ValueSetUsingMicrosoftApi1", entryOptions);
        await distributedCache.SetStringAsync("KeySetUsingMicrosoftApi2", "ValueSetUsingMicrosoftApi2", entryOptions);
    }

    private static async Task CreateSampleDataUsingConnectionMultiplexerAsync(IServiceProvider serviceProvider)
    {
        var connectorFactory = serviceProvider.GetRequiredService<ConnectorFactory<RedisOptions, IConnectionMultiplexer>>();
        Connector<RedisOptions, IConnectionMultiplexer> connector = connectorFactory.GetDefault();

        // Do not dispose the IConnectionMultiplexer singleton.
        IConnectionMultiplexer connectionMultiplexer = connector.GetConnection();
        IDatabase database = connectionMultiplexer.GetDatabase();
        string appName = connectionMultiplexer.ClientName;

        await SetMicrosoftCompatibleStringValue(database, appName, "KeySetUsingRedisApi1", "ValueSetUsingRedisApi1", ExampleSlidingExpiration);
        await SetMicrosoftCompatibleStringValue(database, appName, "KeySetUsingRedisApi2", "ValueSetUsingRedisApi2", ExampleSlidingExpiration);
    }

    private static async Task SetMicrosoftCompatibleStringValue(IDatabase database, string appName, string keyName, string value, TimeSpan? slidingExpiration)
    {
        // Microsoft.Extensions.Caching.StackExchangeRedis.RedisCache is unable to read values of type STRING, so fallback to HASH structure for interop.
        HashEntry[] hashFields = GetHashFields(value, slidingExpiration);

        // Microsoft.Extensions.Caching.StackExchangeRedis.RedisCache silently prefixes all keys with the client name, so replicate that for interop.
        await database.HashSetAsync(appName + keyName, hashFields);
    }

    private static HashEntry[] GetHashFields(string value, TimeSpan? slidingExpiration)
    {
        var hashFields = new List<HashEntry>();

        if (slidingExpiration != null)
        {
            hashFields.Add(new HashEntry("sldexp", slidingExpiration.Value.Ticks));
        }

        hashFields.Add(new HashEntry("data", value));
        return hashFields.ToArray();
    }
}

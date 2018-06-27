

using Microsoft.Extensions.Logging;
using Steeltoe.Management.Endpoint.Health;
using System;
using StackExchange.Redis;

namespace Microsoft.eShopOnContainers.Services.Ordering.API

{
  public class RedisHealthContributor : IHealthContributor
    {
        IConnectionMultiplexer _connectionMultiplexer;
        ILogger<RedisHealthContributor> _logger;
        public RedisHealthContributor(IConnectionMultiplexer connectionMultiplexer, ILogger<RedisHealthContributor> logger)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _logger = logger;
        }

        public string Id { get; } = "redis";

        public Health Health()
        {
            _logger.LogInformation("Checking Redis connection health!");

            Health result = new Health();
            result.Details.Add("cache", "Redis");
            IDatabase _database = null;
            try
            {
                _database = _connectionMultiplexer.GetDatabase();
                if (_database != null && _database.IsConnected("healthcheck")) {
                    result.Details.Add("result", Boolean.TrueString);
                    result.Details.Add("status", HealthStatus.UP.ToString());
                    result.Status = HealthStatus.UP;
                    _logger.LogInformation("Redis connection up!");
                }

            } catch (Exception e)
            {
                _logger.LogInformation("Redis connection down!");
                result.Details.Add("error", e.GetType().Name + ": " + e.Message);
                result.Details.Add("status", HealthStatus.DOWN.ToString());
                result.Status = HealthStatus.DOWN;
            } 

            return result;
        }
    }
}
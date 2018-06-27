using Microsoft.Extensions.Logging;
using Steeltoe.Management.Endpoint.Health;
using System;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBusRabbitMQ;

namespace Microsoft.eShopOnContainers.Services.Catalog.API

{
  public class RabbitMQHealthContributor : IHealthContributor
    {
        IRabbitMQPersistentConnection _connection;
        ILogger<RabbitMQHealthContributor> _logger;
        public RabbitMQHealthContributor(IRabbitMQPersistentConnection connection, ILogger<RabbitMQHealthContributor> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public string Id { get; } = "rabbitMq";

        public Health Health()
        {
            _logger.LogInformation("Checking RabbitMQ connection health!");

            Health result = new Health();
            result.Details.Add("queue", "RabbitMQ");
            
            try
            {
                if (_connection != null && _connection.IsConnected) {
                    result.Details.Add("result", Boolean.TrueString);
                    result.Details.Add("status", HealthStatus.UP.ToString());
                    result.Status = HealthStatus.UP;
                    _logger.LogInformation("RabbitMQ connection up!");
                }

            } catch (Exception e)
            {
                _logger.LogInformation("RabbitMQ connection down!");
                result.Details.Add("error", e.GetType().Name + ": " + e.Message);
                result.Details.Add("status", HealthStatus.DOWN.ToString());
                result.Status = HealthStatus.DOWN;
            } 

            return result;
        }
    }
}
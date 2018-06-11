using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.CircuitBreaker.Hystrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Steeltoe.CircuitBreaker.Ideas
{
   public interface IHystrixCommandBuilder
    {
        IHystrixCommandBuilder WithConfiguration(IConfiguration configuration);
        IHystrixCommandBuilder WithOptions(IHystrixCommandOptions options);

        IHystrixCommandBuilder UseHttpClient(string name);
        IHttpClientBuilder AddHttpClient();
        IHttpClientBuilder AddHttpClient(string name);

        IHttpClientBuilder AddRabbitMQClient(string name);  // This would probably be in the Connectors library

        IHttpClientBuilder AddMySqlConnection(string name);  // This would probably be in the Connectors library

        IHystrixFactory Build();

    }
}

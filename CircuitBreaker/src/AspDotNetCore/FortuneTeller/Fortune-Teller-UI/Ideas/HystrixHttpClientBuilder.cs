using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Steeltoe.CircuitBreaker.Ideas
{
    public class HystrixHttpClientBuilder : IHttpClientBuilder
    {
        public HystrixHttpClientBuilder(IHystrixCommandBuilder commandBuilder, IServiceCollection services, string name)
        {
            CommandBuilder = commandBuilder;
            Services = services;
            Name = name;
        }

        public string Name { get; }

        public IServiceCollection Services { get; }

        public IHystrixCommandBuilder CommandBuilder { get; }
    }
}

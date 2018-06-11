using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Steeltoe.CircuitBreaker.Ideas
{
    public static class IHttpClientBuilderExtensions
    {
        public static IHystrixCommandBuilder Build(this IHttpClientBuilder builder)
        {
            var hystrixClientBuilder = builder as HystrixHttpClientBuilder;
            return hystrixClientBuilder.CommandBuilder;
        }

        public static IHttpClientBuilder AddEurekaDiscovery(this IHttpClientBuilder builder)
        {
            return builder;
        }

        public static IHttpClientBuilder AddRibbonLoadBalancer(this IHttpClientBuilder builder)
        {
            return builder;
        }
        public static IHttpClientBuilder AddSecurityTokenRelay(this IHttpClientBuilder builder)
        {
            return builder;
        }
    }
}

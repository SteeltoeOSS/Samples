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
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Steeltoe.CircuitBreaker.Ideas
{
    public static class HystrixFactoryServiceCollectionExtensions
    {
        public static IHystrixFactory AddHystrixFactory(this IServiceCollection services)
        {
            return null;
        }

    }
}

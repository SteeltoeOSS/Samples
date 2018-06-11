using Steeltoe.CircuitBreaker.Hystrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Steeltoe.CircuitBreaker.Ideas
{
    public interface IHystrixCollapserBuilder
    {
        IHystrixCollapserBuilder WithOptions(IHystrixCollapserOptions options);
        IHystrixFactory Build();
    }
}

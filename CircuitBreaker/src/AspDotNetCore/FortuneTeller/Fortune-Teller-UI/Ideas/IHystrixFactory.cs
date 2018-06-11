using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Steeltoe.CircuitBreaker.Ideas
{ 
    public interface IHystrixFactory
    {
        IHystrixFactory WithConfiguration(IConfiguration configuration);


        IHystrixFactory WithOptions(IHystrixFactoryOptions options);


        IHystrixCommandBuilder AddHystrixCommand<TService, TServiceImpl>();


        IHystrixCollapserBuilder AddHystrixCollapser<TService, TServiceImpl>();


        IHystrixCommandBuilder AddHystrixCommand<TService, TServiceImpl>(string name);


        IHystrixCollapserBuilder AddHystrixCollapser<TService, TServiceImpl>( string name);
 
    }
}

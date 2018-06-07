using Fortune_Teller_UI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Client;
using Steeltoe.CircuitBreaker.Hystrix;
using Steeltoe.Common.Http;
using Steeltoe.Common.Http.Discovery;
using System;

namespace Fortune_Teller_UI
{
    public class Startup
    {
        private ILoggerFactory _loggerFactory;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            _loggerFactory = loggerFactory;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDiscoveryClient(Configuration);

            // A Hystrix command that makes use of the FortuneService
            services.AddHystrixCommand<FortuneServiceCommand>("FortuneService", Configuration);

            // Add some pretend services that make use of the FortuneServiceCollaper to return multiple fortunes
            // Each pretend service receives an injected FortuneServiceCollaper to retrieve a fortune and also
            // calls on another FakeService to get Fortunes from it.  All of this is done async and in parallel.
            // Due to the use of the FortuneServiceCollapser, all of the Fortune requests are batched up by 
            // the collapser and issued in a single request to the backend service
            services.AddTransient<IFakeService1, FakeService1>();
            services.AddTransient<IFakeService2, FakeService2>();
            services.AddTransient<IFakeServices3, FakeService3>();

            // A Hystrix collapser that makes use of the FortuneService(s) above to get a Fortune.
            services.AddHystrixCollapser<IFortuneServiceCollapser, FortuneServiceCollapser>("FortuneServiceCollapser", Configuration);

            /************************************************************************************************************************************/

            // Add HttpMessageHandlers for use with HttpClientFactory
            services.AddTransient<DiscoveryHttpMessageHandler>();
            services.AddTransient<HystrixHttpMessageHandler>();

            // Add two versions of injectible HttpClient via HttpClientFactory (new in ASP.NET Core 2.1)
            // Create a version of HttpClient that comes with a circuit breaker and discovery
            services.AddHttpClient("fortunesWithHystrixHandler", c =>
                {
                    c.BaseAddress = new Uri("http://fortuneService/api/fortunes/");
                })
                    // QUESTION: does a fallback method make sense here? 
                    // N kinds of request will made with this client, perhaps they shouldn't ever share a fallback...
                //.AddCircuitBreaker(loggerFactory: _loggerFactory)
                .AddHystrixCommand<AltRandomFortuneCommand>()
                .AddHttpMessageHandler<DiscoveryHttpMessageHandler>()
                .AddTypedClient<IFortuneService, FortuneService>();
            // Create a version of HttpClient that comes with discovery (for use inside a circuit breaker)
            services.AddHttpClient("fortunesWithoutHystrixHandler", c =>
                {
                    c.BaseAddress = new Uri("http://fortuneService/api/fortunes/");
                })
                .AddHttpMessageHandler<DiscoveryHttpMessageHandler>()
                .AddTypedClient<IFortuneService, FortuneService>();

            /************************************************************************************************************************************/

            // Add framework services.
            services.AddMvc();

            // Add Hystrix metrics stream to enable monitoring 
            services.AddHystrixMetricsStream(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // Add Hystrix Metrics context to pipeline
            app.UseHystrixRequestContext();

            app.UseMvc();
            
            // Startup discovery client
            app.UseDiscoveryClient();

            // Startup Hystrix metrics stream
            app.UseHystrixMetricsStream();
        }

    }
}

using Fortune_Teller_UI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Client;
using Steeltoe.CircuitBreaker.Hystrix;
using Steeltoe.CircuitBreaker.Ideas;

namespace Fortune_Teller_UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDiscoveryClient(Configuration);

            // The Fortune service itself, calls the REST APIs to get random fortunes
            services.AddSingleton<IFortuneService, FortuneService>();

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


            // IDEAS
            services.AddHystrixFactory()
                .WithConfiguration(Configuration)   // Should allow various wayss to configure factory (e.g. configuration, options, action which returns options, etc) ".WithOptions(new IHystrixFactoryOptions())"
                .AddHystrixCommand<ICommand1, Command1>()
                    .WithConfiguration(Configuration)  // Should allow various ways to configure each command (e.g. configuration, options, action which returns options, etc) ".WithOptions(new HystrixCommandOptions())"
                    .AddHttpClient("clientName")
                        .ConfigureHttpClient((client) => { client.Timeout = new System.TimeSpan(5000); })
                        .AddEurekaDiscovery()
                        .AddRibbonLoadBalancer()
                        .Build()
                    .Build()
                .AddHystrixCommand<ICommand2, Command2>()
                    .WithOptions(new HystrixCommandOptions(HystrixCommandKeyDefault.AsKey("foobar")))
                    .AddRabbitMQClient("rabbitName")
                    .Build()
                .AddHystrixCommand<ICommand3, Command3>()
                    .WithOptions(new HystrixCommandOptions(HystrixCommandKeyDefault.AsKey("bar")) { CircuitBreakerRequestVolumeThreshold = 10 })
                    .WithOptions(new HystrixThreadPoolOptions(HystrixThreadPoolKeyDefault.AsKey("key1")) { CoreSize = 5 })
                    .AddMySqlConnection("mysqlName")
                    .Build()
                .AddHystrixCollapser<ICollapser1, Collapser1>()
                    .WithOptions(new HystrixCollapserOptions(HystrixCollapserKeyDefault.AsKey("foobar")) { MaxRequestsInBatch = 5 })
                    .Build();
            // END IDEAS

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

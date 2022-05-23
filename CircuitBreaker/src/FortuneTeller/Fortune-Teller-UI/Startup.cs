using Fortune_Teller_UI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.CircuitBreaker.Hystrix;
using Microsoft.Extensions.Hosting;

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

            // Add framework services.
            services.AddControllersWithViews();

            // To view your metrics stream in the Hystrix dashboard, add Hystrix metrics stream to enable monitoring
            services.AddHystrixMetricsStream(Configuration);
            // To view your Hystrix metrics in an environment where the Hystrix dashboard is not available, use the Hystrix Metrics Event source.
            //services.AddHystrixMetricsEventSource();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }

    }
}

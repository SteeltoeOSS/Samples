using Fortune_Teller_UI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Steeltoe.Common.Http.Discovery;
using Steeltoe.Management.Exporter.Tracing;
using Steeltoe.Management.Tracing;
using System;

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
            services.AddHttpClient("fortunes", c =>
                {
                    c.BaseAddress = new Uri("http://fortuneService/api/fortunes/");
                })
                .AddHttpMessageHandler<DiscoveryHttpMessageHandler>()
                .AddTypedClient<IFortuneService, FortuneService>();

            // Add Distributed tracing
            services.AddDistributedTracing(Configuration);

            // Export traces to Zipkin
            services.AddZipkinExporter(Configuration);

            // Add framework services.
#if NETCOREAPP3_0
            services.AddControllersWithViews();
#else
            services.AddMvc();
#endif
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

#if NETCOREAPP3_0
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
#else
            app.UseMvc();
#endif

            // Start up trace exporter
            app.UseTracingExporter();
        }
    }
}

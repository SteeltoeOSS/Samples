using Fortune_Teller_UI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Common.Http.Discovery;
using System;
using Steeltoe.Management.Endpoint.Health;
using Steeltoe.Management.Endpoint;

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
            services.AddHealthActuator(Configuration);
            services.AddHttpClient("fortunes", c =>
                {
                    c.BaseAddress = new Uri("http://fortuneService/api/fortunes/");
                })
                .AddServiceDiscovery()
                .AddTypedClient<IFortuneService, FortuneService>();

            // Add framework services.
            services.AddControllersWithViews();
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
            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapDefaultControllerRoute();
                endpoints.Map<HealthEndpoint>();
            });

            HealthStartupFilter.InitializeAvailability(app.ApplicationServices);
        }
    }
}

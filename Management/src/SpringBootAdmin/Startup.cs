using CloudFoundry;
using idunno.Authentication.Basic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Management.Endpoint.Info;
using Steeltoe.Management.Endpoint.SpringBootAdminClient;
using Steeltoe.Management.Info;
using System.Security.Claims;

namespace SpringBootAdmin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
         
            // Add your own IInfoContributor, making sure to register with the interface
            services.AddSingleton<IInfoContributor, ArbitraryInfoContributor>();

            // Add your own IHealthContributor, registered with the interface
            services.AddSingleton<IHealthContributor, CustomHealthContributor>();

            services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
               .AddBasicAuth("/actuator", new Claim("scope", "actuators.read"));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("actuators.read", policy => policy.RequireClaim("scope", "actuators.read"));
            });

            // Add framework services.
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
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
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Start up the metrics forwarder service added above
            // Remove comment below to enable
            // app.UseMetricsExporter();
            app.RegisterWithSpringBootAdmin(Configuration);
        }
    }
}

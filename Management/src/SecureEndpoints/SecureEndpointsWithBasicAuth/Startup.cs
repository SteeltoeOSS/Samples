using idunno.Authentication.Basic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SecureEndpoints;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Info;
using System.Security.Claims;

namespace SecureWithBasic
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
                   .AddBasicAuth("/actuator", new Claim("scope", "actuators.read"));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("actuators.read", policy => policy.RequireClaim("scope", "actuators.read"));
            });

            // services.AddAllActuators(Configuration); Optionally you can add them manually (and map below)

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

                //  endpoints.MapAllActuators().RequireAuthorization("actuators.read"); If you use this, make sure to uncomment AddAllActuators above
            });
        }
    }
}

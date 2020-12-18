using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleCloudFoundry.Model;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Configuration.ConfigServer;

namespace SimpleCloudFoundry
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
            services.AddOptions();

            // Optional: Adds ConfigServerClientOptions to service container
            services.ConfigureConfigServerClientOptions(Configuration);

            // Optional:  Adds CloudFoundryApplicationOptions and CloudFoundryServicesOptions to service container
            services.ConfigureCloudFoundryOptions(Configuration);

            // Adds the configuration data POCO configured with data returned from the Spring Cloud Config Server
            services.Configure<ConfigServerData>(Configuration);

#if NETCOREAPP3_1 || NET5_0
            services.AddControllersWithViews();
#else
            services.AddMvc();
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#if NETCOREAPP3_1 || NET5_0
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
#else
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
#endif
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

#if NETCOREAPP3_1 || NET5_0
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
#else
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
#endif
        }
    }
}

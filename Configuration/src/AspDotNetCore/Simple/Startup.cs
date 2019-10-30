using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Simple.Model;
using Steeltoe.Extensions.Configuration.ConfigServer;

namespace Simple
{
    //
    // NOTE: This sample application assumes a running Spring Cloud Config Server is started
    //       with repository data for application named: foo, & profile: development
    //
    //       The easiest way to get that to happen is clone the spring-cloud-config
    //       repo and run the config-server.
    //          eg. git clone https://github.com/spring-cloud/spring-cloud-config.git
    //              cd spring-cloud-config\spring-cloud-config-server
    //              mvn spring-boot:run
    //

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

            // Optional:  Adds IConfiguration and IConfigurationRoot to service container
            services.AddConfiguration(Configuration);

            // Add framework services.
#if NETCOREAPP3_0
            services.AddControllersWithViews();
#else
            services.AddMvc();
#endif

            // Adds the configuration data POCO configured with data returned from the Spring Cloud Config Server
            services.Configure<ConfigServerData>(Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

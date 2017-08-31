
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Pivotal.Extensions.Configuration;
using Pivotal.Discovery.Client;
using MusicStore.Models;

using Steeltoe.CloudFoundry.Connector.MySql.EFCore;
using Steeltoe.Management.Endpoint.Health;
using Steeltoe.Management.CloudFoundry;
using Steeltoe.Extensions.Logging.CloudFoundry;

namespace MusicStore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddConfigServer(env, loggerFactory);

            Configuration = builder.Build();

            loggerFactory.AddCloudFoundry(Configuration.GetSection("Logging"));
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Add custom health check contributor
            services.AddSingleton<IHealthContributor, MySqlHealthContributor>();

            // Add managment endpoint services
            services.AddCloudFoundryActuators(Configuration);

            // Add framework services.
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddMvc();

            services.AddDiscoveryClient(Configuration);

            services.AddDbContext<MusicStoreContext>(options => options.UseMySql(Configuration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
 
            // Add management endpoints into pipeline
            app.UseCloudFoundryActuators();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

                routes.MapRoute(
                    name: "api",
                    template: "{controller}/{id?}");
            });

            app.UseDiscoveryClient();

            SampleData.InitializeMusicStoreDatabase(app.ApplicationServices);
        }
    }
}

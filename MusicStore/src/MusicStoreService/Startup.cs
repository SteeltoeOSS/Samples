using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicStore.Models;
using Steeltoe.Connector.SqlServer.EFCore;
using Steeltoe.Common;
using Steeltoe.Discovery.Client;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.CloudFoundry;
using Steeltoe.Management.Endpoint.Env;
using Steeltoe.Management.Endpoint.Refresh;
using Steeltoe.Management.Tracing;

namespace MusicStore
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
            // this should be done automatically by Steeltoe somewhere else! Zipkin throws without it
            if (Platform.IsCloudFoundry)
            {
                services.RegisterCloudFoundryApplicationInstanceInfo();
            }
            else
            {
                services.GetApplicationInstanceInfo();
            }

            // Add Steeltoe Management services
            services.AddCloudFoundryActuators(Configuration);
            services.AddEnvActuator(Configuration);
            services.AddRefreshActuator(Configuration);

            // Add framework services.
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Steeltoe Service Discovery
            if (!Configuration.GetValue<bool>("DisableServiceDiscovery"))
            {
                services.AddDiscoveryClient(Configuration);
            }

            // Steeltoe MySQL Connector
            services.AddDbContext<MusicStoreContext>(options => options.UseSqlServer(Configuration));

            services.AddDistributedTracing(Configuration, builder => builder.UseZipkinWithTraceOptions(services));

            // Add Framework services
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            // Add Steeltoe Management endpoints into pipeline
            app.UseCloudFoundryActuators();
            app.UseEnvActuator();
            app.UseRefreshActuator();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Start Steeltoe Discovery services
            if (!Configuration.GetValue<bool>("DisableServiceDiscovery"))
            {
                app.UseDiscoveryClient();
            }
        }
    }
}

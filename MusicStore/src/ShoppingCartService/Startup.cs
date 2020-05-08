using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCartService.Models;
using Steeltoe.Common;
using Steeltoe.Connector.SqlServer.EFCore;
using Steeltoe.Discovery.Client;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.CloudFoundry;
using Steeltoe.Management.Endpoint.Env;
using Steeltoe.Management.Endpoint.Refresh;
using Steeltoe.Management.Tracing;

namespace ShoppingCartService
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

            // Add managment endpoint services
            services.AddCloudFoundryActuators(Configuration);
            services.AddEnvActuator(Configuration);
            services.AddRefreshActuator(Configuration);

            // Add framework services.
            services.AddControllers();

            if (!Configuration.GetValue<bool>("DisableServiceDiscovery"))
            {
                services.AddDiscoveryClient(Configuration);
            }
            else
            {
                services.AddConfigurationDiscoveryClient(Configuration);
            }

            services.AddDistributedTracing(Configuration, builder => builder.UseZipkinWithTraceOptions(services));

            // var cstring = new ConnectionStringManager(Configuration).Get<SqlServerConnectionInfo>().ConnectionString;
            // Console.WriteLine("Using SQL Connection: {0}", cstring);
            // services.AddDbContext<ShoppingCartContext>(options => options.UseSqlServer(cstring));
            services.AddDbContext<ShoppingCartContext>(options => options.UseSqlServer(Configuration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            
            // Add management endpoints into pipeline
            app.UseCloudFoundryActuators();
            app.UseEnvActuator();
            app.UseRefreshActuator();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            if (!Configuration.GetValue<bool>("DisableServiceDiscovery"))
            {
                app.UseDiscoveryClient();
            }
        }
    }
}

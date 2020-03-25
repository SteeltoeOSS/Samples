using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCartService.Models;
using Steeltoe.CloudFoundry.Connector.SqlServer.EFCore;
using Steeltoe.Discovery.Client;
using Steeltoe.Management.CloudFoundry;
using Steeltoe.Management.Endpoint.Env;
using Steeltoe.Management.Endpoint.Refresh;
//using Steeltoe.Management.Exporter.Tracing;
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

            services.AddDistributedTracing(Configuration);
//            services.AddZipkinExporter(Configuration);
            
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

            
//            app.UseTracingExporter();
            
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

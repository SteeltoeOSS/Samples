using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.CloudFoundry.Connector.EFCore;
using Steeltoe.CloudFoundry.Connector.MySql;
using Steeltoe.CloudFoundry.Connector.MySql.EFCore;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Management.Endpoint.Info;
using Steeltoe.Management.Endpoint.Metrics;
using Steeltoe.Management.TaskCore;

namespace CloudFoundry
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
            // Add entity framework db context bound to connection string in configuration
            services.AddDbContext<MyContext>(options => options.UseMySql(Configuration));
            // Add MySql health contributor to be exposed by the endpoint
            services.AddMySqlHealthContributor(Configuration);
            
            // register a migrate context task with PCF
            services.AddTask<MigrateDbContextTask<MyContext>>();

            // Add your own IInfoContributor, making sure to register with the interface
            services.AddSingleton<IInfoContributor, ArbitraryInfoContributor>();

            // Add your own IHealthContributor, registered with the interface
            services.AddSingleton<IHealthContributor, CustomHealthContributor>();

            services.AddMetricsActuator(Configuration);
            // Add management components which collect and forwards metrics to 
            // the Cloud Foundry Metrics Forwarder service
            // services.AddMetricsForwarderExporter(Configuration);

            // Add framework services.
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

            // Add metrics collection to the app
            // Remove comment below to enable
            // app.UseMetricsActuator();
#if NETCOREAPP3_1 || NET5_0
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
#else
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
#endif
            // Start up the metrics forwarder service added above
            // Remove comment below to enable
            // app.UseMetricsExporter();
        }
    }
}

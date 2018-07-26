using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Steeltoe.CloudFoundry.Connector.MySql;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Management.CloudFoundry;
using Steeltoe.Management.Endpoint.Env;
using Steeltoe.Management.Endpoint.Info;
using Steeltoe.Management.Endpoint.Metrics;
using Steeltoe.Management.Exporter.Metrics;

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
            // Add in a MySql connection (this method also adds an IHealthContributor for it)
            services.AddMySqlConnection(Configuration);

            // Add managment endpoint services
            services.AddCloudFoundryActuators(Configuration);

            // Add your own IInfoContributor, making sure to register with the interface
            services.AddSingleton<IInfoContributor, ArbitraryInfoContributor>();

            // Add your own IHealthContributor, registered with the interface
            services.AddSingleton<IHealthContributor, CustomHealthContributor>();

            // Add management components which collect and forwards metrics to 
            // the Cloud Foundry Metrics Forwarder service
            // Remove comments below to enable
            // services.AddMetricsActuator(Configuration);
            // services.AddMetricsForwarderExporter(Configuration);

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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

            // Add management endpoints into pipeline
            app.UseCloudFoundryActuators();

            // Add metrics collection to the app
            // Remove comment below to enable
            // app.UseMetricsActuator();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Start up the metrics forwarder service added above
            // Remove comment below to enable
            // app.UseMetricsExporter();
        }
    }
}

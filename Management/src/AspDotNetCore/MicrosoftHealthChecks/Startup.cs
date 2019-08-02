using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.MySql;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Management.CloudFoundry;
using Steeltoe.Management.Endpoint.Info;

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
            // Add microsoft community health checks 
            var cm = new ConnectionStringManager(Configuration);
            var connectionString = cm.Get<MySqlConnectionInfo>().ConnectionString;
            services.AddHealthChecks().AddMySql(connectionString); 

            // Add in a MySql connection (this method also adds an IHealthContributor for it)
            services.AddMySqlConnection(Configuration); //will use microsoft health check instead of steelto health check

            // Add managment endpoint services
            services.AddCloudFoundryActuators(Configuration); // can check health from microsoft health check on the health actuator 

            services.AddHealthChecksUI();

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
            //Optionally use Microsoft health middleware for MsftHealth Checks
            // at /Health
            app.UseHealthChecks("/Health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            //Optionally use health checks ui at /healthchecks-ui
            app.UseHealthChecksUI();

            // Add management endpoints into pipeline
            // Microsoft health check shows up at /cloudfoundryapplication/health

            app.UseCloudFoundryActuators();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}

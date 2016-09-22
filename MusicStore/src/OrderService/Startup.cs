
using Pivotal.Extensions.Configuration;
using Pivotal.Discovery.Client;

#if NET451 && MYSQL
using Steeltoe.CloudFoundry.Connector.MySql.EF6;
#endif

#if !NET451 || POSTGRES
using Steeltoe.CloudFoundry.Connector.PostgreSql.EFCore;
#endif

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OrderService.Models;

namespace OrderService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddConfigServer(env);
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Add framework services.
            services.AddMvc();

            services.AddDiscoveryClient(Configuration);
#if NET451 && MYSQL
            services.AddDbContext<OrdersContext>(Configuration);
#endif
#if !NET451 || POSTGRES
            services.AddDbContext<OrdersContext>(options => options.UseNpgsql(Configuration));
#endif

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            app.UseDiscoveryClient();

            SampleData.InitializeOrderDatabaseAsync(app.ApplicationServices).Wait();
        }
    }
}

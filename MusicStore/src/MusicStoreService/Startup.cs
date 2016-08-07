
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Pivotal.Extensions.Configuration;
using Pivotal.Discovery.Client;
using MusicStore.Models;

#if NET451 && MYSQL
using SteelToe.CloudFoundry.Connector.MySql.EF6;
#endif
#if !NET451 || POSTGRES
using SteelToe.CloudFoundry.Connector.PostgreSql.EFCore;
#endif

namespace MusicStore
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

            services.AddMvc();

            services.AddDiscoveryClient(Configuration);
#if NET451 && MYSQL
            services.AddDbContext<MusicStoreContext>(Configuration);
#endif
#if !NET451 || POSTGRES
            services.AddDbContext<MusicStoreContext>(options => options.UseNpgsql(Configuration));
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

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

            SampleData.InitializeMusicStoreDatabaseAsync(app.ApplicationServices).Wait();
        }
    }
}

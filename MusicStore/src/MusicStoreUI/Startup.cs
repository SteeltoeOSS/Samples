
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pivotal.Extensions.Configuration;
using Pivotal.Discovery.Client;

#if USE_REDIS_CACHE
using Steeltoe.CloudFoundry.Connector.Redis;
using Steeltoe.Security.DataProtection;
using Microsoft.AspNetCore.DataProtection;
#endif

using MusicStoreUI.Services;
using MusicStoreUI.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;


using Steeltoe.CloudFoundry.Connector.MySql.EFCore;
using Steeltoe.Extensions.Logging.CloudFoundry;
using Steeltoe.Management.CloudFoundry;
using Steeltoe.Management.Endpoint.Health;

namespace MusicStoreUI
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

            // Add framework services.
#if USE_REDIS_CACHE
            services.AddRedisConnectionMultiplexer(Configuration);
            services.AddDataProtection()
                .PersistKeysToRedis()
                .SetApplicationName("MusicStoreUI");
            services.AddDistributedRedisCache(Configuration);
#else
            services.AddDistributedMemoryCache();
#endif
            // Add custom health check contributor
            services.AddSingleton<IHealthContributor, MySqlHealthContributor>();

            // Add managment endpoint services
            services.AddCloudFoundryActuators(Configuration);

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddDbContext<AccountsContext>(options => options.UseMySql(Configuration));
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                    {
                        options.Cookies.ApplicationCookie.AccessDeniedPath = "/Home/AccessDenied";
                    })
                    .AddEntityFrameworkStores<AccountsContext>()
                    .AddDefaultTokenProviders();

            services.AddDiscoveryClient(Configuration);

            services.AddSingleton<IMusicStore, MusicStoreService>();
            services.AddSingleton<IShoppingCart, ShoppingCartService>();
            services.AddSingleton<IOrderProcessing, OrderProcessingService>();

            services.AddLogging();

            services.AddMvc();

            // Add memory cache services
            services.AddMemoryCache();

            // Add session related services.

            // Use call below if you want sticky Sessions on Cloud Foundry
            // services.AddSession((options) => options.CookieName = "JSESSIONID");

            services.AddSession();

            // Configure Auth
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "ManageStore",
                    authBuilder =>
                    {
                        authBuilder.RequireClaim("ManageStore", "Allowed");
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Add management endpoints into pipeline
            app.UseCloudFoundryActuators();

            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // Add cookie-based authentication to the request pipeline
            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action}",
                    defaults: new { action = "Index" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseDiscoveryClient();

            SampleData.InitializeAccountsDatabase(app.ApplicationServices, Configuration);

        }
    }
}

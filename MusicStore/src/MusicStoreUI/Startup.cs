using MusicStoreUI.Services;
using MusicStoreUI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.CloudFoundry.Connector.SqlServer.EFCore;
using Steeltoe.Discovery.Client;

#if USE_REDIS_CACHE
using Microsoft.AspNetCore.DataProtection;
using Steeltoe.CloudFoundry.Connector.Redis;
using Steeltoe.Security.DataProtection;
#endif

using Steeltoe.Management.CloudFoundry;
using Steeltoe.CircuitBreaker.Hystrix;
using Command = MusicStoreUI.Services.HystrixCommands;

namespace MusicStoreUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

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

            // Add managment endpoint services
            services.AddCloudFoundryActuators(Configuration);

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddDbContext<AccountsContext>(options => options.UseSqlServer(Configuration));
            services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "/Home/AccessDenied");
            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<AccountsContext>()
                    .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/LogIn");

            if (Configuration.GetValue<bool>("DisableServiceDiscovery"))
            {
                services.AddConfigurationDiscoveryClient(Configuration);
            }
            else
            {
                services.AddDiscoveryClient(Configuration);
            }

            services.AddSingleton<IMusicStore, MusicStoreService>();
            services.AddSingleton<IShoppingCart, ShoppingCartService>();
            services.AddSingleton<IOrderProcessing, OrderProcessingService>();

            services.AddHystrixCommand<Command.GetTopAlbums>("MusicStore", Configuration);
            services.AddHystrixCommand<Command.GetGenres>("MusicStore", Configuration);
            services.AddHystrixCommand<Command.GetGenre>("MusicStore", Configuration);
            services.AddHystrixCommand<Command.GetAlbum>("MusicStore", Configuration);

            services.AddControllersWithViews();

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
                        authBuilder.RequireAuthenticatedUser();
                    });
            });

            // Add Hystrix metrics stream to enable monitoring
            services.AddHystrixMetricsStream(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Add Hystrix Metrics context to pipeline
            app.UseHystrixRequestContext();

            // Add management endpoints into pipeline
            app.UseCloudFoundryActuators();

            app.UseSession();

            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            // Add cookie-based authentication to the request pipeline
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areaRoute",
                    pattern: "{area:exists}/{controller}/{action}",
                    defaults: new { action = "Index" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            if (!Configuration.GetValue<bool>("DisableServiceDiscovery"))
            {
                app.UseDiscoveryClient();
            }

            // Startup Hystrix metrics stream
            app.UseHystrixMetricsStream();
        }
    }
}

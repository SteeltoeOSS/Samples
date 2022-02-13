using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicStoreUI.Models;
using MusicStoreUI.Services;
using Steeltoe.CircuitBreaker.Hystrix;
using Steeltoe.Common.Http.Discovery;
using Steeltoe.Connector.MySql.EFCore;
using Steeltoe.Connector.Redis;
using Steeltoe.Management.Tracing;
using Steeltoe.Security.DataProtection;
using System;
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
            bool.TryParse(Environment.GetEnvironmentVariable("USE_REDIS_CACHE"), out var useRedis);
            if (useRedis)
            {
                services.AddRedisConnectionMultiplexer(Configuration);
                services.AddDataProtection()
                    .PersistKeysToRedis()
                    .SetApplicationName("MusicStoreUI");
                services.AddDistributedRedisCache(Configuration);
            }
            else
            {
                Console.WriteLine("NOT Using Redis");
                services.AddDistributedMemoryCache();
            }

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // services.AddDbContext<AccountsContext>(options => options.UseSqlServer(cstring));
            services.AddDbContext<AccountsContext>(options => options.UseMySql(Configuration));
            services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "/Home/AccessDenied");
            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<AccountsContext>()
                    .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/LogIn");

            services.AddDistributedTracing();

            services
                .AddHttpClient<IMusicStore, MusicStoreService>()
                .AddServiceDiscovery();
            services
                .AddHttpClient<IShoppingCart, ShoppingCartService>()
                .AddServiceDiscovery();
            services
                .AddHttpClient<IOrderProcessing, OrderProcessingService>()
                .AddServiceDiscovery();


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
        }
    }
}

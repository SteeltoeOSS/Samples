using Autofac;
using Autofac.Extensions.DependencyInjection;
using IdentityServer4.Services;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.ServiceFabric;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.Identity.API.Certificates;
using Microsoft.eShopOnContainers.Services.Identity.API.Data;
using Microsoft.eShopOnContainers.Services.Identity.API.Models;
using Microsoft.eShopOnContainers.Services.Identity.API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using Steeltoe.CloudFoundry.Connector.MySql.EFCore;
using Steeltoe.CloudFoundry.Connector.Redis;
using Steeltoe.CloudFoundry.ConnectorAutofac;
using Steeltoe.Security.DataProtection;
using Steeltoe.Management.Endpoint.Health;
using Steeltoe.Management.CloudFoundry;
using Microsoft.AspNetCore.HttpOverrides;

namespace Microsoft.eShopOnContainers.Services.Identity.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            RegisterAppInsights(services);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("https://apps.run.pcfbeta.io"));
            });

            services.AddRedisConnectionMultiplexer(Configuration);

            // Add framework services.
            Action<EntityFrameworkCore.Infrastructure.MySqlDbContextOptionsBuilder> mySqlOptionsAction = (o) =>
                                        {
                                            o.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                            //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                            o.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                        };
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseMySql(Configuration, mySqlOptionsAction));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add custom health check contributor
            services.AddScoped<IHealthContributor, MySqlHealthContributor>();
            services.AddScoped<IHealthContributor, RedisHealthContributor>();

            // Add management endpoint services
            services.AddCloudFoundryActuators(Configuration);

            services.Configure<AppSettings>(Configuration);

            services.AddMvc();

            if (Configuration.GetValue<string>("IsClusterEnv") == bool.TrueString)
            {
               services.AddDataProtection()
                .PersistKeysToRedis()
                .SetApplicationName("IdentityApi");
            }

            services.AddTransient<ILoginService<ApplicationUser>, EFLoginService>();
            services.AddTransient<IRedirectService, RedirectService>();

            // Adds IdentityServer
            services.AddIdentityServer(x => {
                    x.IssuerUri="null"; 
                    x.PublicOrigin="https://identityapi.example.com";
                })
                
                .AddSigningCredential(Certificate.Get())
                .AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseMySql(Configuration,mySqlOptionsAction);
                })
                .AddOperationalStore(options =>
                 {
                     options.ConfigureDbContext = builder => builder.UseMySql(Configuration,mySqlOptionsAction);
                 })
                .Services.AddTransient<IProfileService, ProfileService>();

            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                loggerFactory.CreateLogger("init").LogDebug($"Using PATH BASE '{pathBase}'");
                app.UsePathBase(pathBase);
            }


#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            app.Map("/liveness", lapp => lapp.Run(async ctx => ctx.Response.StatusCode = 200));
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

            app.UseStaticFiles();

            // Make work identity server redirections in Edge and lastest versions of browers. WARN: Not valid in a production environment.
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Security-Policy", "script-src 'unsafe-inline'");
                await next();
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // Adds IdentityServer
            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

             // adds Cloud Foundry Management Actuators
            app.UseCloudFoundryActuators();
        }

        private void RegisterAppInsights(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);
            var orchestratorType = Configuration.GetValue<string>("OrchestratorType");

            if (orchestratorType?.ToUpper() == "K8S")
            {
                // Enable K8s telemetry initializer
                services.EnableKubernetes();
            }
            if (orchestratorType?.ToUpper() == "SF")
            {
                // Enable SF telemetry initializer
                services.AddSingleton<ITelemetryInitializer>((serviceProvider) =>
                    new FabricTelemetryInitializer());
            }
        }
    }
}

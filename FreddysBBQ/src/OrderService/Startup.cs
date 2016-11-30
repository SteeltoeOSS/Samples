using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pivotal.Extensions.Configuration;
using Pivotal.Discovery.Client;
using OrderService.Models;
using Steeltoe.CloudFoundry.Connector.MySql.EFCore;
using Microsoft.AspNetCore.Http;
using Steeltoe.Security.Authentication.CloudFoundry;
using Common.Services;

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
                .AddConfigServer(env)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddCloudFoundryJwtAuthentication(Configuration);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Orders", policy => policy.RequireClaim("scope", "order.me"));
                options.AddPolicy("AdminOrders", policy => policy.RequireClaim("scope", "order.admin"));
            });

            services.AddDiscoveryClient(Configuration);

            services.AddDbContext<OrderContext>(options => options.UseMySql(Configuration));
       
            services.AddSingleton<IMenuService, MenuService>();

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCloudFoundryJwtAuthentication();

            app.UseMvc();

            app.UseDiscoveryClient();

            SampleData.InitializeOrderDatabase(app.ApplicationServices);
        }
    }
}


using FortuneTellerService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Pivotal.Discovery.Client;
using Steeltoe.Extensions.Configuration;
using System;

namespace FortuneTellerService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, ILoggerFactory factory)
        {
            factory.AddConsole(minLevel: LogLevel.Debug);
            
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddCloudFoundry()
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFramework()
                .AddDbContext<FortuneContext>(options => options.UseInMemoryDatabase());

            services.AddSingleton<IFortuneRepository, FortuneRepository>();

            services.AddDiscoveryClient(Configuration);

            services.AddLogging();

            // Add framework services.
            services.AddMvc();
        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime lifetime)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseStaticFiles();

            app.UseMvc();

            app.UseDiscoveryClient();

            SampleData.InitializeFortunesAsync(app.ApplicationServices).Wait();
        }

    }
}

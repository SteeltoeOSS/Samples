using Azure.Cosmos;
using Azure.Cosmos.Fluent;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.CosmosDb;
using System.Threading.Tasks;

namespace CosmosDb
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
            services.AddSingleton<ICosmosDbService>(InitializeCosmosClientInstanceAsync(Configuration).GetAwaiter().GetResult());

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        /// <summary>
        /// Creates a Cosmos DB database and a container with the specified partition key. 
        /// </summary>
        /// <returns></returns>
        /// <remarks><see cref="CosmosDbConnectorOptions"/></remarks>
        private static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(IConfiguration configuration)
        {
            // read settings from "cosmosdb:client" and VCAP:services or services:
            var configMgr = new ConnectionStringManager(configuration);
            var cosmosInfo = configMgr.Get<CosmosDbConnectionInfo>();

            // these are mapped into the properties dictionary
            var databaseName = cosmosInfo.Properties["DatabaseId"];
            var databaseLink = cosmosInfo.Properties["DatabaseLink"];

            // container is not known to be provided by a service binding:
            var containerName = configuration.GetValue<string>("CosmosDb:Container");
            var cosmosClient = new CosmosClient(cosmosInfo.ConnectionString);
            var cosmosDbService = new CosmosDbService(cosmosClient, databaseName, containerName);

            var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

            return cosmosDbService;
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MongoDb.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Steeltoe.Common.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.Endpoint;
using System;
using System.Collections.Generic;

namespace MongoDb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            InitializeMongo(host.Services);
            
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .AddAllActuators()
                .AddCloudFoundryConfiguration()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseCloudHosting();

        private static void InitializeMongo(IServiceProvider services)
        {
            var mongo = services.GetService(typeof(IMongoClient)) as MongoClient;
            var mongoInfo = services.GetService(typeof(MongoUrl)) as MongoUrl;
            var db = mongo.GetDatabase(mongoInfo.DatabaseName ?? "TestData");
            var collection = db.GetCollection<Person>("TestDataCollection");
            collection.InsertMany(
                new List<Person>
                {
                    new Person { Id = ObjectId.GenerateNewId(), FirstName = "Albert", LastName = "Einstein", FavoriteThing = "Relativity" },
                    new Person { Id = ObjectId.GenerateNewId(), FirstName = "Isaac", LastName = "Newton", FavoriteThing = "Gravity" },
                });
        }
    }
}

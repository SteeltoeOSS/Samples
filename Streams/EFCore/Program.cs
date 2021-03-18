using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Steeltoe.Common.Util;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using Steeltoe.Stream.StreamsHost;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EFCore
{
    class Program
    {

        static async Task Main(string[] args)
        {

            await CreateHostBuilder(args)
              .ConfigureServices((context, services) =>
              {
                  // Get from   context.Configuration
                  var connectionString = "server=localhost;user=root;password=my-secret-pw;database=steeltoe";
                  services.AddDbContextPool<FooContext>(
                      dbContextOptions => dbContextOptions
                         .UseMySql(connectionString, mySqlOptions => mySqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend))
                         .EnableSensitiveDataLogging()
                         .EnableDetailedErrors());
                  services.AddLogging(builder =>
                  {
                      builder.AddDebug();
                      builder.AddConsole();
                  });
                  // Add Rabbit template
                  services.AddRabbitTemplate();
                  services.AddHostedService<MyRabbitSender>();
              }).Build().StartAsync();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            StreamsHost.CreateDefaultBuilder<BindableChannels>(args);
        public class FooContext : DbContext
        {
            public FooContext(DbContextOptions<FooContext> options) : base(options)
            { }

            public DbSet<Foo> Foos { get; set; }
        }
        public class Foo
        {
            [Key]
            public int id { get; set; }
            public string name { get; set; }
            public string tag { get; set; }
        }

        [EnableBinding(typeof(ISink))]
        public class BindableChannels
        {
            private DbContext _db;

            AtomicBoolean _semaphore = new AtomicBoolean(true);
            public BindableChannels(FooContext db)
            {
                _db = db;

            }

            [StreamListener("input")]
            public void HandleInputMessage(Foo foo)
            {
                Console.Write("\n Received foo " + foo.id + foo.name + foo.tag);
                _db.Database.EnsureCreated();
                _db.Add(foo);
                _db.SaveChanges();
            }


        }
    }

}

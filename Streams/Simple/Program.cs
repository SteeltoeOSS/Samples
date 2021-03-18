using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Common.Contexts;
using Steeltoe.Common.Lifecycle;
using Steeltoe.Messaging;
using Steeltoe.Messaging.Support;
using Steeltoe.Stream.Binder;
using Steeltoe.Stream.Binding;
using Steeltoe.Stream.Extensions;
using Steeltoe.Stream.Messaging;
using Steeltoe.Stream.StreamsHost;

namespace steeltoe_stream_samples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateStreamHostBuilder(args).Build(); // Alternatively use the default host builder

            host.Run();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            StreamsHost.CreateDefaultBuilder<SomeClass>(args)
            .ConfigureWebHostDefaults(webhostBuilder => webhostBuilder.UseStartup<WebStartup>());

        public static IHostBuilder CreateStreamHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
          .ConfigureWebHostDefaults(webhostBuilder => webhostBuilder.UseStartup<WebStartup>())
           .AddStreamsServices<SomeClass>();

    }
}

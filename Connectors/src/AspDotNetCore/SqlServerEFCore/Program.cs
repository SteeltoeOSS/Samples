using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;

namespace SqlServerEFCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel()
                .UseUrls(GetServerUrls(args))
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            //return WebHost.CreateDefaultBuilder(args)
            //        .UseKestrel()
            //        .UseUrls(GetServerUrls(args))
            //        .UseStartup<Startup>()
            //        .Build();
        }

        private static string[] GetServerUrls(string[] args)
        {
            List<string> urls = new List<string>();
            for (int i = 0; i < args.Length; i++)
            {
                if ("--server.urls".Equals(args[i], StringComparison.OrdinalIgnoreCase))
                {
                    urls.Add(args[i + 1]);
                }
            }
            return urls.ToArray();
        }
    }
}

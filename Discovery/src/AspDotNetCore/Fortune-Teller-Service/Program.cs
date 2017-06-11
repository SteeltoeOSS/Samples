using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;

// Remove comments to enable SSL settings for 
// using System.Security.Cryptography.X509Certificates;
// using Microsoft.AspNetCore.Server.Kestrel.Https;

namespace FortuneTellerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // var cert = new X509Certificate2("testcert.pfx", "password");
            var host = new WebHostBuilder()
               .UseKestrel()
            //Use these kestrel settings if wanting to use HTTPS AND C2C networking on Cloud Foundry
            //    .UseKestrel(cfg => cfg.UseHttps(new HttpsConnectionFilterOptions()
            //    {
            //        ServerCertificate = cert,
            //        ClientCertificateValidation = (a, b, c) => { return true; },
            //        ClientCertificateMode = ClientCertificateMode.AllowCertificate
            //    }))
                .UseUrls(GetServerUrls(args))
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
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

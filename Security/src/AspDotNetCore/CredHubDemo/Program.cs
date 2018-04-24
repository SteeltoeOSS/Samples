using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Logging;
using Steeltoe.Security.DataProtection.CredHubCore;
using System;
using System.Collections.Generic;
using System.IO;

namespace CredHubDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            //File.WriteAllText("instance.crt", "-----BEGIN CERTIFICATE-----\n...\n-----END CERTIFICATE-----\n");
            //File.WriteAllText("instance.key", "-----BEGIN RSA PRIVATE KEY-----\n...\n-----END RSA PRIVATE KEY-----\n");
            //Environment.SetEnvironmentVariable("CF_INSTANCE_CERT", "instance.crt");
            //Environment.SetEnvironmentVariable("CF_INSTANCE_KEY", "instance.key");

            // forcefully setup credentials that need to be interpolated
            Environment.SetEnvironmentVariable("VCAP_SERVICES", @"
                { 
                    ""p-demo-resource"": [
                    {
                    ""credentials"": {
                        ""credhub-ref"": ""((/config-server/credentials))""
                    },
                    ""label"": ""p-config-server"",
                    ""name"": ""config-server"",
                    ""plan"": ""standard"",
                    ""provider"": null,
                    ""syslog_drain_url"": null,
                    ""tags"": [
                        ""configuration"",
                        ""spring-cloud""
                    ],
                    ""volume_mounts"": []
                    }]
                }");

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseCloudFoundryHosting()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.SetBasePath(builderContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{builderContext.HostingEnvironment.EnvironmentName}.json", optional: true)
                        .AddCloudFoundry()
                        .AddEnvironmentVariables();
                })
                .ConfigureLogging((builderContext, loggingBuilder) =>
                {
                    loggingBuilder.AddConfiguration(builderContext.Configuration.GetSection("Logging"));
                    loggingBuilder.AddDynamicConsole();
                })
                .UseCredHubInterpolation(new LoggerFactory().AddConsole())
                .Build();

            host.Run();
        }
    }
}

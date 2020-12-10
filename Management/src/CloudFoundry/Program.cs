using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Steeltoe.Common.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Logging.DynamicSerilog;
using Steeltoe.Management.CloudFoundry;
using Steeltoe.Management.TaskCore;

namespace CloudFoundry
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = WebHost.CreateDefaultBuilder()
                .AddCloudFoundryConfiguration()              // config
                .UseCloudHosting()    // listen on port defined in env var 'PORT'
                .AddDynamicSerilog()
                .AddCloudFoundryActuators()     // add actuators - should come AFTER Serilog config or else DynamicConsoleLogger will be injected
                .UseStartup<Startup>()
                .Build();

           host.RunWithTasks();
        }
    }
}

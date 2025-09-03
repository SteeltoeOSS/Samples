using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Steeltoe.Management.Endpoint;

namespace SecureEndpointsWithUAA;

public class Program
{
    public static void Main(string[] args)
    {
        BuildWebHost(args).Run();
    }

    public static IHost BuildWebHost(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHost(configure => configure.UseStartup<Startup>().UseKestrel())
            .AddAllActuators(endpoints => endpoints.RequireAuthorization("actuators.read"))
            .Build();
}
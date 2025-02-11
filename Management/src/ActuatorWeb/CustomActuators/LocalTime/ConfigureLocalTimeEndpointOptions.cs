using Steeltoe.Management.Endpoint.Configuration;

namespace Steeltoe.Samples.ActuatorWeb.CustomActuators.LocalTime;

internal sealed class ConfigureLocalTimeEndpointOptions(IConfiguration configuration)
    : ConfigureEndpointOptions<LocalTimeEndpointOptions>(configuration, "Management:Endpoints:LocalTime", "local-time")
{
    public override void Configure(LocalTimeEndpointOptions options)
    {
        base.Configure(options);

        options.Format ??= "O";
    }
}

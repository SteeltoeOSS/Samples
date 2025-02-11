using Microsoft.Extensions.Options;
using Steeltoe.Management.Endpoint.Configuration;
using Steeltoe.Management.Endpoint.Middleware;

namespace Steeltoe.Samples.ActuatorWeb.CustomActuators.LocalTime;

internal sealed class LocalTimeEndpointMiddleware(
    ILocalTimeEndpointHandler endpointHandler, IOptionsMonitor<ManagementOptions> managementOptionsMonitor, ILoggerFactory loggerFactory)
    : EndpointMiddleware<object?, string>(endpointHandler, managementOptionsMonitor, loggerFactory)
{
    protected override async Task<string> InvokeEndpointHandlerAsync(HttpContext context, CancellationToken cancellationToken)
    {
        return await EndpointHandler.InvokeAsync(null, cancellationToken);
    }
}

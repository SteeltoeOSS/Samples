using System.Globalization;
using Microsoft.Extensions.Options;
using Steeltoe.Management.Configuration;

namespace Steeltoe.Samples.ActuatorWeb.CustomActuators.LocalTime;

internal sealed class LocalTimeEndpointHandler : ILocalTimeEndpointHandler
{
    private readonly IOptionsMonitor<LocalTimeEndpointOptions> _optionsMonitor;

    public EndpointOptions Options => _optionsMonitor.CurrentValue;

    public LocalTimeEndpointHandler(IOptionsMonitor<LocalTimeEndpointOptions> optionsMonitor)
    {
        ArgumentNullException.ThrowIfNull(optionsMonitor);

        _optionsMonitor = optionsMonitor;
    }

    public Task<string> InvokeAsync(object? argument, CancellationToken cancellationToken)
    {
        string localTime = GetLocalTime();
        return Task.FromResult(localTime);
    }

    private string GetLocalTime()
    {
        return DateTime.Now.ToString(_optionsMonitor.CurrentValue.Format, CultureInfo.InvariantCulture);
    }
}

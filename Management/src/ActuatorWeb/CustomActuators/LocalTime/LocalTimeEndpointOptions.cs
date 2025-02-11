using Steeltoe.Management.Configuration;

namespace Steeltoe.Samples.ActuatorWeb.CustomActuators.LocalTime;

public sealed class LocalTimeEndpointOptions : EndpointOptions
{
    /// <summary>
    /// Gets or sets the date/time format to use in the response. Defaults to "O".
    /// </summary>
    /// <remarks>
    /// See <see href="https://learn.microsoft.com/dotnet/standard/base-types/standard-date-and-time-format-strings" /> and
    /// <see href="https://learn.microsoft.com/dotnet/standard/base-types/custom-date-and-time-format-strings" /> for possible values.
    /// </remarks>
    public string? Format { get; set; }
}

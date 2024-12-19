using Steeltoe.Management.Endpoint;

namespace Steeltoe.Samples.ActuatorWeb.CustomActuators.LocalTime;

public interface ILocalTimeEndpointHandler : IEndpointHandler<object?, string>;

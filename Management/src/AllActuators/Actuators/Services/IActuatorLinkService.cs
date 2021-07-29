using Steeltoe.Actuators.Models;
using Steeltoe.Management.Endpoint.Hypermedia;
using System.Collections.Generic;

namespace Steeltoe.Actuators.Services
{
    public interface IActuatorLinkService
    {
        IEnumerable<HrefProperties> GetActuatorLinks();
    }
}

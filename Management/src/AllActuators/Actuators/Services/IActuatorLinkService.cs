using Steeltoe.Actuators.Models;
using System.Collections.Generic;

namespace Steeltoe.Actuators.Services
{
    public interface IActuatorLinkService
    {
        IEnumerable<HrefProperties> GetActuatorLinks();
    }
}

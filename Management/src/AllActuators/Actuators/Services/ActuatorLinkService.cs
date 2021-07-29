using Microsoft.Extensions.Logging;
using Steeltoe.Actuators.Models;
using Steeltoe.Management.Endpoint.Hypermedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Steeltoe.Actuators.Services
{
    public class ActuatorLinkService : IActuatorLinkService
    {
        private readonly ActuatorEndpoint actuatorEndpoint;
        private readonly ILogger<ActuatorLinkService> logger;

        public ActuatorLinkService(ActuatorEndpoint actuatorEndpoint, ILogger<ActuatorLinkService> logger)
        {
            this.actuatorEndpoint = actuatorEndpoint;
            this.logger = logger;
        }

        public IEnumerable<HrefProperties> GetActuatorLinks()
        {
            logger.LogInformation("Retrieving actuators");

            var actuatorLinks = Enumerable.Empty<HrefProperties>();

            var actuatorEndpoints = actuatorEndpoint.Invoke("/actuator");

            if (actuatorEndpoints is not null)
            {
                logger.LogInformation($"Found {actuatorEndpoints._links.Count} actuators");

                actuatorLinks = actuatorEndpoints._links.Select(link =>
                    new HrefProperties
                    {
                        Display = link.Key != "self" ? link.Key : "all actuators",
                        Address = link.Value.Href
                    });
            }
            else
            {
                logger.LogError("No actuators were found");
            }

            return actuatorLinks;
        }
    }
}

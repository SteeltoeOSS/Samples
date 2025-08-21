using Console.Common;
using Microsoft.Extensions.Logging;
using Steeltoe.Common.Contexts;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Listener;

namespace EndpointRegistration;

public class MyRabbitEndpointConfigurer(IApplicationContext context, ILoggerFactory loggerFactory)
    : IRabbitListenerConfigurer
{
    public void ConfigureRabbitListeners(IRabbitListenerEndpointRegistrar registrar)
    {
        var listener = new SampleMessageListener(loggerFactory.CreateLogger<SampleMessageListener>());
        var endpoint = new SimpleRabbitListenerEndpoint(context, listener) { Id = "manual-endpoint" };
        endpoint.SetQueueNames("myQueue");
        registrar.RegisterEndpoint(endpoint);
    }
}
using Console.Common;
using Microsoft.Extensions.Logging;
using Steeltoe.Common.Contexts;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Listener;

namespace EndpointRegistration;

public class MyRabbitEndpointConfigurer : IRabbitListenerConfigurer
{
    private readonly IApplicationContext _context;
    private readonly ILoggerFactory _loggerFactory;

    public MyRabbitEndpointConfigurer(IApplicationContext context, ILoggerFactory loggerFactory)
    {
        _context = context;
        _loggerFactory = loggerFactory;
    }

    public void ConfigureRabbitListeners(IRabbitListenerEndpointRegistrar registrar)
    {
        var listener = new SampleMessageListener(_loggerFactory.CreateLogger<SampleMessageListener>());
        var endpoint = new SimpleRabbitListenerEndpoint(_context, listener) { Id = "manual-endpoint" };
        endpoint.SetQueueNames("myQueue");
        registrar.RegisterEndpoint(endpoint);
    }
}
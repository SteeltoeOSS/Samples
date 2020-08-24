using ConsoleGenericHost;
using Microsoft.Extensions.Logging;
using Steeltoe.Common.Contexts;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Listener;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericHostEndpointRegistration
{
    public class MyRabbitEndpointConfigurer : IRabbitListenerConfigurer
    {
        private IApplicationContext context;
        private ILoggerFactory loggerFactory;

        public MyRabbitEndpointConfigurer(IApplicationContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            this.loggerFactory = loggerFactory;
        }

        public void ConfigureRabbitListeners(IRabbitListenerEndpointRegistrar registrar)
        {
            var listener = new MyMessageListener(loggerFactory.CreateLogger<MyMessageListener>());
            SimpleRabbitListenerEndpoint endpoint = new SimpleRabbitListenerEndpoint(context, listener);
            endpoint.Id = "manual-endpoint";
            endpoint.SetQueueNames("myqueue");
            registrar.RegisterEndpoint(endpoint);
        }
    }
}

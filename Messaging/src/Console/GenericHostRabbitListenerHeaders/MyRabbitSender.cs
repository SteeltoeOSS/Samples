using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging;
using Steeltoe.Messaging.RabbitMQ.Connection;
using Steeltoe.Messaging.RabbitMQ.Core;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Messaging.RabbitMQ.Support;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleGenericHost
{
    public class MyRabbitSender : IHostedService
    {
        private RabbitTemplate template;
        private Timer timer;
        private int counter = 1;

        private RabbitDestination destination = new RabbitDestination("myqueue");

        public MyRabbitSender(IServiceProvider services)
        {
            template = services.GetRabbitTemplate();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(Sender, null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Dispose();
            return Task.CompletedTask;
        }

        private void Sender(object state)
        {
            Order order = new Order() { OrderNumber = counter++, OrderType = "myType" };
            var headers = new Dictionary<string, object>() { { "order_type", "myType" } };
            template.ConvertAndSend(destination, order, headers);
        }
    }
}

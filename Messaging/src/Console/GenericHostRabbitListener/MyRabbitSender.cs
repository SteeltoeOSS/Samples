using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging.RabbitMQ.Core;
using Steeltoe.Messaging.RabbitMQ.Extensions;
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
            template.ConvertAndSend("myqueue", "foo");
        }
    }
}

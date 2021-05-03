using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Steeltoe.Messaging;
using Steeltoe.Messaging.RabbitMQ.Core;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Messaging.Support;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static EFCore.Program;

namespace EFCore
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
           
            template.ConvertAndSend("sample-sink-data","", new Foo() { name = "test", tag = "tag1" });
        }
    }
}

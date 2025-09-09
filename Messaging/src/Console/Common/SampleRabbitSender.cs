using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging;
using Steeltoe.Messaging.RabbitMQ.Core;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using System.Text;

namespace Console.Common;

public class SampleRabbitSender : IHostedService
{
    private readonly RabbitTemplate _template;
    private Timer _timer;

    public SampleRabbitSender(IServiceProvider services)
    {
        _template = services.GetRabbitTemplate();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(Sender!, null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Dispose();
        return Task.CompletedTask;
    }

    private void Sender(object state)
    {
        _template.Send("myQueue", Message.Create(Encoding.UTF8.GetBytes("foo")));
    }
}
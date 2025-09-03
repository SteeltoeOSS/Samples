using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging;
using Steeltoe.Messaging.RabbitMQ.Core;
using Steeltoe.Messaging.RabbitMQ.Extensions;

namespace Console.Common;

public class SampleRabbitSender(IServiceProvider services) : IHostedService
{
    private readonly RabbitTemplate _template = services.GetRabbitTemplate();
    private Timer? _timer;

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
        _template.Send("myQueue", Message.Create("foo"u8.ToArray()));
    }
}
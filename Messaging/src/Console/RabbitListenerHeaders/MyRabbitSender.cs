using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging.RabbitMQ.Core;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitListenerHeaders;

public class MyRabbitSender : IHostedService
{
    private readonly RabbitTemplate _template;
    private Timer _timer;
    private int _counter = 1;

    private readonly RabbitDestination _destination = new("myQueue");

    public MyRabbitSender(IServiceProvider services)
    {
        _template = services.GetRabbitTemplate();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(Sender, null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Dispose();
        return Task.CompletedTask;
    }

    private void Sender(object state)
    {
        var order = new Order { OrderNumber = _counter++, OrderType = "myType" };
        var headers = new Dictionary<string, object> { { "order_type", "myType" } };
        _template.ConvertAndSend(_destination, order, headers);
    }
}
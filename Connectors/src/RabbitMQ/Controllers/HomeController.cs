using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using Steeltoe.Connectors;
using Steeltoe.Connectors.RabbitMQ;
using Steeltoe.Samples.RabbitMQ.Models;

namespace Steeltoe.Samples.RabbitMQ.Controllers;

public sealed class HomeController(ConnectorFactory<RabbitMQOptions, IConnection> connectorFactory) : Controller
{
    private const string RabbitQueueName = "rabbit-test-queue";
    private const string RabbitExchangeName = "rabbit-test-exchange";
    private const string RabbitRoutingKey = "rabbit-test-routing-key";

    private readonly Connector<RabbitMQOptions, IConnection> _connector = connectorFactory.Get();

    public IActionResult Index()
    {
        return View(new RabbitViewModel
        {
            ConnectionString = _connector.Options.ConnectionString
        });
    }

    public async Task<IActionResult> Send(string? messageToSend, CancellationToken cancellationToken)
    {
        // Steeltoe: Send RabbitMQ message to the queue.

        if (string.IsNullOrEmpty(messageToSend))
        {
            return View("Index", new RabbitViewModel
            {
                ConnectionString = _connector.Options.ConnectionString,
                SendStatus = RabbitSendStatus.Failed
            });
        }

        // Do not dispose the IConnection singleton.
        IConnection connection = _connector.GetConnection();
        await using IChannel channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
        await CreateQueueAsync(channel, cancellationToken);

        byte[] body = Encoding.UTF8.GetBytes(messageToSend);
        await channel.BasicPublishAsync(RabbitExchangeName, RabbitRoutingKey, false, new BasicProperties(), body, cancellationToken);

        return View("Index", new RabbitViewModel
        {
            ConnectionString = _connector.Options.ConnectionString,
            SendStatus = RabbitSendStatus.Succeeded
        });
    }

    public async Task<IActionResult> Receive(CancellationToken cancellationToken)
    {
        // Steeltoe: Receive RabbitMQ message from the queue. Do not dispose the IConnection singleton.

        IConnection connection = _connector.GetConnection();
        await using IChannel channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
        await CreateQueueAsync(channel, cancellationToken);

        BasicGetResult? result = await channel.BasicGetAsync(RabbitQueueName, true, cancellationToken);

        if (result == null)
        {
            return View("Index", new RabbitViewModel
            {
                ConnectionString = _connector.Options.ConnectionString,
                MessageReceived = string.Empty
            });
        }

        string messageReceived = Encoding.UTF8.GetString(result.Body.ToArray());

        return View("Index", new RabbitViewModel
        {
            ConnectionString = _connector.Options.ConnectionString,
            MessageReceived = messageReceived
        });
    }

    private static async Task CreateQueueAsync(IChannel channel, CancellationToken cancellationToken)
    {
        await channel.ExchangeDeclareAsync(RabbitExchangeName, ExchangeType.Direct, cancellationToken: cancellationToken);
        await channel.QueueDeclareAsync(RabbitQueueName, false, false, false, cancellationToken: cancellationToken);
        await channel.QueueBindAsync(RabbitQueueName, RabbitExchangeName, RabbitRoutingKey, cancellationToken: cancellationToken);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}

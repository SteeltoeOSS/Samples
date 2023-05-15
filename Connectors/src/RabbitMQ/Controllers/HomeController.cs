using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Models;
using Steeltoe.Connectors;
using Steeltoe.Connectors.RabbitMQ;

namespace RabbitMQ.Controllers;

public class HomeController : Controller
{
    private const string RabbitQueueName = "rabbit-test";

    private readonly ILogger<HomeController> _logger;
    private readonly ConnectionProvider<RabbitMQOptions, IConnection> _connectionProvider;

    public HomeController(ILogger<HomeController> logger, ConnectionFactory<RabbitMQOptions, IConnection> connectionFactory)
    {
        _logger = logger;
        _connectionProvider = connectionFactory.GetDefault();
    }

    public IActionResult Index()
    {
        return View(new RabbitViewModel
        {
            ConnectionString = _connectionProvider.Options.ConnectionString
        });
    }

    public IActionResult Send(string? messageToSend)
    {
        // Steeltoe: Send RabbitMQ message to the queue.

        if (string.IsNullOrEmpty(messageToSend))
        {
            return View("Index", new RabbitViewModel
            {
                ConnectionString = _connectionProvider.Options.ConnectionString,
                SendStatus = RabbitSendStatus.Failed
            });
        }

        // Do not dispose the IConnection singleton.
        IConnection connection = _connectionProvider.GetConnection();
        using IModel channel = connection.CreateModel();

        CreateQueue(channel);

        byte[] body = Encoding.UTF8.GetBytes(messageToSend);
        channel.BasicPublish("", RabbitQueueName, null, body);

        return View("Index", new RabbitViewModel
        {
            ConnectionString = _connectionProvider.Options.ConnectionString,
            SendStatus = RabbitSendStatus.Succeeded
        });
    }

    public IActionResult Receive()
    {
        // Steeltoe: Receive RabbitMQ message from the queue. Do not dispose the IConnection singleton.

        IConnection connection = _connectionProvider.GetConnection();
        using IModel channel = connection.CreateModel();

        CreateQueue(channel);

        BasicGetResult? result = channel.BasicGet(RabbitQueueName, true);

        if (result == null)
        {
            return View("Index", new RabbitViewModel
            {
                ConnectionString = _connectionProvider.Options.ConnectionString,
                MessageReceived = string.Empty
            });
        }

        string messageReceived = Encoding.UTF8.GetString(result.Body.ToArray());

        return View("Index", new RabbitViewModel
        {
            ConnectionString = _connectionProvider.Options.ConnectionString,
            MessageReceived = messageReceived
        });
    }

    private static void CreateQueue(IModel channel)
    {
        channel.QueueDeclare(RabbitQueueName, false, false, false, null);
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

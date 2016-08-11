using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rabbit.Controllers
{
    public class RabbitController : Controller
    {
        ConnectionFactory _rabbitConnection;

        public RabbitController([FromServices] ConnectionFactory rabbitConnection)
        {
            _rabbitConnection = rabbitConnection;
        }

    
        public IActionResult Receive()
        {
            using (var connection = _rabbitConnection.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                CreateQueue(channel);
                var data = channel.BasicGet("rabbit-test", true);
                if (data != null) {
                    ViewData["message"] = Encoding.UTF8.GetString(data.Body);
                }
            }

            return View();
        }

        public IActionResult Send(string message)
        {
            if (message != null && message != "") {
                using (var connection = _rabbitConnection.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    CreateQueue(channel);
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "",
                                         routingKey: "rabbit-test",
                                         basicProperties: null,
                                         body: body);
                }
            }
            return View();
        }

        protected void CreateQueue(IModel channel)
        {
            channel.QueueDeclare(queue: "rabbit-test",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
        }
    }
}

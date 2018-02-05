using System.Security.Authentication;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Net.Security;

namespace RabbitMQ.Controllers
{
    public class RabbitMQController : Controller
    {
        ConnectionFactory _rabbitConnection;

        public RabbitMQController([FromServices] ConnectionFactory rabbitConnection)
        {
            _rabbitConnection = rabbitConnection;
            SslOption opt = _rabbitConnection.Ssl;
            if (opt != null && opt.Enabled)
            {
                opt.Version = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12;

                // Only needed if want to disable certificate validations
                opt.AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateChainErrors | 
                    SslPolicyErrors.RemoteCertificateNameMismatch | SslPolicyErrors.RemoteCertificateNotAvailable;
            }
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

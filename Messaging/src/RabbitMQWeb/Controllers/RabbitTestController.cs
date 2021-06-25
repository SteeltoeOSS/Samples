using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Core;

namespace RabbitMQWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RabbitTestController : ControllerBase
    {
        private readonly ILogger<RabbitTestController> _logger;
        private readonly RabbitTemplate _rabbitTemplate;
        private readonly RabbitAdmin _rabbitAdmin;

        public RabbitTestController(ILogger<RabbitTestController> logger, RabbitTemplate rabbitTemplate, RabbitAdmin rabbitAdmin)
        {
            _logger = logger;
            _rabbitTemplate = rabbitTemplate;
            _rabbitAdmin = rabbitAdmin;
        }

        [HttpGet()]
        public ActionResult<string> Index()
        {
            return @"
You can use these endpoints to interact with RabbitMQ
    /RabbitTest/SendFoo
    /RabbitTest/SendBar
    /RabbitTest/SendReceiveBar
    /RabbitTest/SendReceiveFoo
    /RabbitTest/DeleteQueues
";
        }

        [HttpGet("sendfoo")]
        public ActionResult<string> SendFoo()
        {
            var foo = new Foo("send foo string");
            _rabbitTemplate.ConvertAndSend(Queues.InferredFooQueue, foo);
            _logger.LogInformation("SendFoo: Sent message to " + Queues.InferredFooQueue);
            return "Message sent ... look at logs to see if message processed by RabbitListenerService";
        }

        [HttpGet("sendbar")]
        public ActionResult<string> SendBar()
        {
            var bar = new Bar("send bar string");
            _rabbitTemplate.ConvertAndSend(Queues.InferredBarQueue, bar);
            _logger.LogInformation("SendBar: Sent message to " + Queues.InferredBarQueue);
            return "Message sent ... look at logs to see if message processed by RabbitListenerService";
        }

        [HttpGet("sendreceivefoo")]
        public ActionResult<string> SendReceiveFoo()
        {
            var foo = new Foo("SendReceiveFoo foo string");
            _rabbitTemplate.ConvertAndSend(Queues.ReceiveAndConvertQueue, foo);
            _logger.LogInformation("SendReceiveFoo: Sent message to " + Queues.ReceiveAndConvertQueue);
            foo = _rabbitTemplate.ReceiveAndConvert<Foo>(Queues.ReceiveAndConvertQueue, 10_000);
            _logger.LogInformation("SendReceiveFoo: Received a Foo message back {Message}", foo);
            return foo.ToString();
        }

        [HttpGet("sendreceivebar")]
        public ActionResult<string> SendReceiveBar()
        {
            var bar = new Bar("SendReceiveBar bar string");
            _rabbitTemplate.ConvertAndSend(Queues.ReceiveAndConvertQueue, bar);
            _logger.LogInformation("SendReceiveBar: Sent message to " + Queues.ReceiveAndConvertQueue);
            bar = _rabbitTemplate.ReceiveAndConvert<Bar>(Queues.ReceiveAndConvertQueue, 10_000);
            _logger.LogInformation("SendReceiveBar:Received a Bar message back {Message}", bar);
            return bar.ToString();
        }

        [HttpGet("deletequeues")]
        public ActionResult<string> DeleteQueues()
        {
            _rabbitAdmin.DeleteQueue(Queues.ReceiveAndConvertQueue);
            _logger.LogInformation("DeleteQueue: Deleted queue: " + Queues.ReceiveAndConvertQueue);
            return ("Delete queue complete\n ... All done!");
        }
    }
}

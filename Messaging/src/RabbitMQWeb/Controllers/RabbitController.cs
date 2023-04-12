using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQWeb.Models;
using Steeltoe.Messaging.RabbitMQ.Core;

namespace RabbitMQWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RabbitController : ControllerBase
    {
        private readonly ILogger<RabbitController> _logger;
        private readonly RabbitTemplate _rabbitTemplate;
        private readonly RabbitAdmin _rabbitAdmin;

        public RabbitController(ILogger<RabbitController> logger, RabbitTemplate rabbitTemplate,
            RabbitAdmin rabbitAdmin)
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
    /Rabbit/SendRabbitMessage
    /Rabbit/SendLongEaredRabbitMessage
    /Rabbit/SendReceiveRabbitMessage
    /Rabbit/SendReceiveLongEaredRabbitMessage
    /Rabbit/DeleteQueues
    /Rabbit/QuorumQueue
";
        }

        [HttpGet("SendRabbitMessage")]
        public ActionResult<string> SendRabbitMessage()
        {
            var msg = new RabbitMessage("I'm a rabbit");
            _rabbitTemplate.ConvertAndSend(Queues.InferredRabbitQueue, msg);
            _logger.LogInformation("SendRabbitMessage: sent message to {Queue}", Queues.InferredRabbitQueue);
            return "RabbitMessage sent ... look at logs to see if message processed by a RabbitListener";
        }

        [HttpGet("SendLongEaredRabbitMessage")]
        public ActionResult<string> SendLongEaredRabbitMessage()
        {
            var msg = new LongEaredRabbitMessage("I have long ears");
            _rabbitTemplate.ConvertAndSend(Queues.InferredLongEaredRabbitQueue, msg);
            _logger.LogInformation("SendLongEaredRabbitMessage: sent message to {Queue}",
                Queues.InferredLongEaredRabbitQueue);
            return "LongEaredRabbitMessage sent ... look at logs to see if message processed by a RabbitListener";
        }

        [HttpGet("SendReceiveRabbitMessage")]
        public ActionResult<string> SendReceiveRabbitMessage()
        {
            var msg = new RabbitMessage("hopping to and fro");
            _rabbitTemplate.ConvertAndSend(Queues.SendReceiveRabbitQueue, msg);
            _logger.LogInformation("SendReceiveRabbitMessage: sent \"{Message}\" -> {Queue}", msg,
                Queues.SendReceiveRabbitQueue);
            msg = _rabbitTemplate.ReceiveAndConvert<RabbitMessage>(Queues.SendReceiveRabbitQueue, 10_000);
            _logger.LogInformation("SendReceiveRabbitMessage: received \"{Message}\" <- {Queue}", msg,
                Queues.SendReceiveRabbitQueue);
            return msg.ToString();
        }

        [HttpGet("SendReceiveLongEaredRabbitMessage")]
        public ActionResult<string> SendReceiveLongEaredRabbitMessage()
        {
            var msg = new LongEaredRabbitMessage("flopping my ears to and fro");
            _rabbitTemplate.ConvertAndSend(Queues.SendReceiveRabbitQueue, msg);
            _logger.LogInformation("SendReceiveLongEaredRabbitMessage: sent \"{Message}\" -> {Queue}", msg,
                Queues.SendReceiveRabbitQueue);
            msg = _rabbitTemplate.ReceiveAndConvert<LongEaredRabbitMessage>(Queues.SendReceiveRabbitQueue, 10_000);
            _logger.LogInformation("SendReceiveLongEaredRabbitMessage: received \"{Message}\" <- {Queue}", msg,
                Queues.SendReceiveRabbitQueue);
            return msg.ToString();
        }

        [HttpGet("DeleteQueues")]
        public ActionResult<string> DeleteQueues()
        {
            _rabbitAdmin.DeleteQueue(Queues.SendReceiveRabbitQueue);
            _logger.LogInformation("DeleteQueue: Deleted queue {Queue}", Queues.SendReceiveRabbitQueue);
            return ("Delete queue complete\n ... All done!");
        }
        [HttpGet("QuorumQueue")]
        public ActionResult<string> QuorumQueue()
        {
            var msg = new RabbitMessage("hopping to and fro, quorum ");
            _rabbitTemplate.ConvertAndSend(Queues.QuorumQueue, msg);
            _logger.LogInformation("SendReceiveRabbitMessage: sent \"{Message}\" -> {Queue}", msg,
                Queues.QuorumQueue);
            msg = _rabbitTemplate.ReceiveAndConvert<RabbitMessage>(Queues.QuorumQueue, 10_000);
            _logger.LogInformation("SendReceiveRabbitMessage: received \"{Message}\" <- {Queue}", msg,
                Queues.QuorumQueue);
            return msg.ToString();
        }
    }
}
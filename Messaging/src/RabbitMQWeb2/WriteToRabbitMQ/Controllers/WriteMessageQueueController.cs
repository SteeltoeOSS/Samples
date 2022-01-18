using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Core;

namespace WriteToRabbitMQ.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WriteMessageQueueController : ControllerBase
    {
        public const string RECEIVE_AND_CONVERT_QUEUE = "steeltoe_message_queue";
        private readonly ILogger<WriteMessageQueueController> _logger;
        private readonly RabbitTemplate _rabbitTemplate;
        private readonly RabbitAdmin _rabbitAdmin;

        public WriteMessageQueueController(ILogger<WriteMessageQueueController> logger, RabbitTemplate rabbitTemplate, RabbitAdmin rabbitAdmin)
        {
            _logger = logger;
            _rabbitTemplate = rabbitTemplate;
            _rabbitAdmin = rabbitAdmin;
        }

        [HttpGet()]
        public ActionResult<string> Index()
        {
            var msg = new Message() { Name = "Hi there from over here." };

            _rabbitTemplate.ConvertAndSend(RECEIVE_AND_CONVERT_QUEUE, msg);

            _logger.LogInformation($"Sending message '{msg}' to queue '{RECEIVE_AND_CONVERT_QUEUE}'");

            return "Message sent to queue.";
        }
    }
}

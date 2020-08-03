using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQWeb.Services;
using Steeltoe.Messaging.Rabbit.Core;

namespace RabbitMQWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RabbitTestController : ControllerBase
    {
        public const string RECEIVE_AND_CONVERT_QUEUE = "sample1.receive.and.convert";
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
            return "You can use these endpoints to interact with RabbitMQ\n/RabbitTest/SendFoo \n/RabbitTest/SendBar \n/RabbitTest/SendReceiveBar \n/RabbitTest/SendReceiveFoo \n/RabbitTest/DeleteQueues";
        }

        [HttpGet("sendfoo")]
        public ActionResult<string> SendFoo()
        {
            var foo = new Foo("send foo string");
            _rabbitTemplate.ConvertAndSend(RabbitListenerService.INFERRED_FOO_QUEUE, foo);
            _logger.LogInformation("SendFoo: Sent message to " + RabbitListenerService.INFERRED_FOO_QUEUE);
            return "Message sent ... look at logs to see if message processed by RabbitListenerService";
        }

        [HttpGet("sendbar")]
        public ActionResult<string> SendBar()
        {
            var bar = new Bar("send bar string");
            _rabbitTemplate.ConvertAndSend(RabbitListenerService.INFERRED_BAR_QUEUE, bar);
            _logger.LogInformation("SendBar: Sent message to " + RabbitListenerService.INFERRED_BAR_QUEUE);
            return "Message sent ... look at logs to see if message processed by RabbitListenerService";
        }

        [HttpGet("sendreceivefoo")]
        public ActionResult<string> SendReceiveFoo()
        {
            var foo = new Foo("sendreceivefoo foo string");
            _rabbitTemplate.ConvertAndSend(RECEIVE_AND_CONVERT_QUEUE, foo);
            _logger.LogInformation("SendReceiveFoo: Sent message to " + RECEIVE_AND_CONVERT_QUEUE);
            foo = _rabbitTemplate.ReceiveAndConvert<Foo>(RECEIVE_AND_CONVERT_QUEUE, 10_000);
            _logger.LogInformation("SendReceiveFoo:Receeived a Foo message back " + foo);
            return foo.ToString();
        }

        [HttpGet("sendreceivebar")]
        public ActionResult<string> SendReceiveBar()
        {
            var bar = new Bar("sendreceivebar bar string");
            _rabbitTemplate.ConvertAndSend(RECEIVE_AND_CONVERT_QUEUE, bar);
            _logger.LogInformation("SendReceiveBar: Sent message to " + RECEIVE_AND_CONVERT_QUEUE);
            bar = _rabbitTemplate.ReceiveAndConvert<Bar>(RECEIVE_AND_CONVERT_QUEUE, 10_000);
            _logger.LogInformation("SendReceiveBar:Receeived a Bar message back " + bar);
            return bar.ToString();
        }

        [HttpGet("deletequeues")]
        public ActionResult<string> DeleteQueues()
        {
            _rabbitAdmin.DeleteQueue(RECEIVE_AND_CONVERT_QUEUE);
            _logger.LogInformation("DeleteQueue: Deleted queue: " + RECEIVE_AND_CONVERT_QUEUE);
            return ("Delete queue complete\n ... All done!");
        }
    }
}

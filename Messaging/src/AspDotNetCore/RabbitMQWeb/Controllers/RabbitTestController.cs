using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQWeb.Services;
using Steeltoe.Messaging.Rabbit.Data;
using Steeltoe.Messaging.Rabbit.Core;

namespace RabbitMQWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RabbitTestController : ControllerBase
    {
        public const string RECEIVE_AND_CONVERT_QUEUE = "sample1.receive.and.convert";
        private readonly ILogger<RabbitTestController> _logger;
        private readonly Message _jsonMessage;
        private readonly RabbitTemplate _rabbitTemplate;
        private readonly IAmqpAdmin _rabbitAdmin;

        public RabbitTestController(ILogger<RabbitTestController> logger, RabbitTemplate rabbitTemplate, IAmqpAdmin rabbitAdmin)
        {
            _logger = logger;
            _rabbitTemplate = rabbitTemplate;
            _rabbitAdmin = rabbitAdmin;
            var json = "{\"value\" : \"value\" }";
            var bytes = Encoding.UTF8.GetBytes(json);
            _jsonMessage = MessageBuilder.WithBody(bytes)
                    .AndProperties(MessagePropertiesBuilder.NewInstance().SetContentType("application/json").Build())
                    .Build();
        }

        [HttpGet("sendfoo")]
        public ActionResult<string> SendFoo()
        {
            _rabbitTemplate.Send(RabbitListenerService.INFERRED_FOO_QUEUE, _jsonMessage);
            _logger.LogInformation("SendFoo: Sent message to " + RabbitListenerService.INFERRED_FOO_QUEUE);
            return "Message sent";
        }

        [HttpGet("sendbar")]
        public ActionResult<string> SendBar()
        {
            _rabbitTemplate.Send(RabbitListenerService.INFERRED_BAR_QUEUE, _jsonMessage);
            _logger.LogInformation("SendBar: Sent message to " + RabbitListenerService.INFERRED_BAR_QUEUE);
            return "Message sent";
        }

        [HttpGet("sendreceivefoo")]
        public ActionResult<string> SendReceiveFoo()
        {

            _rabbitTemplate.Send(RECEIVE_AND_CONVERT_QUEUE, _jsonMessage);
            _logger.LogInformation("SendReceiveFoo: Sent message to " + RECEIVE_AND_CONVERT_QUEUE);
            var foo = _rabbitTemplate.ReceiveAndConvert<Foo>(RECEIVE_AND_CONVERT_QUEUE, 10_000, typeof(Foo));
            _logger.LogInformation("SendReceiveFoo:Receeived a Foo message back " + foo);
            return foo.ToString();
        }

        [HttpGet("sendreceivebar")]
        public ActionResult<string> SendReceiveBar()
        {

            _rabbitTemplate.Send(RECEIVE_AND_CONVERT_QUEUE, _jsonMessage);
            _logger.LogInformation("SendReceiveBar: Sent message to " + RECEIVE_AND_CONVERT_QUEUE);
            var bar = _rabbitTemplate.ReceiveAndConvert<Foo>(RECEIVE_AND_CONVERT_QUEUE, 10_000, typeof(Bar));
            _logger.LogInformation("SendReceiveBar:Receeived a Foo message back " + bar);
            return bar.ToString();
        }

        [HttpGet("deletequeue")]
        public ActionResult<string> DeleteQueue()
        {
            _rabbitAdmin.DeleteQueue(RECEIVE_AND_CONVERT_QUEUE);
            _logger.LogInformation("DeleteQueue: Deleted queue: " + RECEIVE_AND_CONVERT_QUEUE);
            return ("Delete queue complete\n ... All done!");
        }
    }
}

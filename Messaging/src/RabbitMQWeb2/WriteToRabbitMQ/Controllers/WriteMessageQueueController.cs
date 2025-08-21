using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Core;

namespace WriteToRabbitMQ.Controllers;

[ApiController]
[Route("[controller]")]
public class WriteMessageQueueController(ILogger<WriteMessageQueueController> logger, RabbitTemplate rabbitTemplate)
    : ControllerBase
{
    private static int _messageCounter = 1;
    [HttpGet]
    public ActionResult<string> Index()
    {
        var msg = new Message { Name = $"Hi there from over here! This is message #{_messageCounter}" };
        rabbitTemplate.ConvertAndSend(Constants.ReceiveAndConvertQueue, msg);

        logger.LogInformation("Sending message '{Message}' to queue '{ReceiveAndConvertQueue}'", msg, Constants.ReceiveAndConvertQueue);

        var returnMessage = $"Message #{_messageCounter} sent to queue.";
        _messageCounter++;
        return returnMessage;
    }
}
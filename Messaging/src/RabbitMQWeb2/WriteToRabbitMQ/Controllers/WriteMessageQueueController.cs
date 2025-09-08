using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Core;

namespace WriteToRabbitMQ.Controllers;

[ApiController]
[Route("[controller]")]
public class WriteMessageQueueController : ControllerBase
{
    private readonly ILogger<WriteMessageQueueController> _logger;
    private readonly RabbitTemplate _rabbitTemplate;

    public WriteMessageQueueController(ILogger<WriteMessageQueueController> logger, RabbitTemplate rabbitTemplate)
    {
        _logger = logger;
        _rabbitTemplate = rabbitTemplate;
    }

    private static int _messageCounter = 1;
    [HttpGet]
    public ActionResult<string> Index()
    {
        var msg = new Message { Name = $"Hi there from over here! This is message #{_messageCounter}" };
        _rabbitTemplate.ConvertAndSend(Constants.ReceiveAndConvertQueue, msg);

        _logger.LogInformation("Sending message '{Message}' to queue '{ReceiveAndConvertQueue}'", msg, Constants.ReceiveAndConvertQueue);

        var returnMessage = $"Message #{_messageCounter} sent to queue.";
        _messageCounter++;
        return returnMessage;
    }
}
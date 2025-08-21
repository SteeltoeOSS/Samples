using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQWeb.Models;
using Steeltoe.Messaging.RabbitMQ.Core;

namespace RabbitMQWeb.Controllers;

[ApiController]
[Route("[controller]")]
public class RabbitController(
    ILogger<RabbitController> logger,
    RabbitTemplate rabbitTemplate,
    RabbitAdmin rabbitAdmin) : ControllerBase
{
    [HttpGet]
    public ActionResult Index()
    {
        return new ContentResult
        {
            ContentType = "text/html",
            Content = """
                      <html>
                       <body>
                       You can use these endpoints to interact with RabbitMQ:<br>
                          <a href="./SendRabbitMessage">Send RabbitMessage</a><br>
                          <a href="./SendLongEaredRabbitMessage">Send LongEaredRabbitMessage</a><br>
                          <a href="./SendReceiveRabbitMessage">Send and receive with RabbitMessage</a><br>
                          <a href="./SendReceiveLongEaredRabbitMessage">Send and receive with LongEaredRabbitMessage</a><br>
                          <a href="./DeleteQueues">Delete Queues</a><br>
                          <a href="./QuorumQueue">Send and receive with Quorum Queue</a><br>
                       </body>
                      </html>
                      """
        };
    }

    [HttpGet("SendRabbitMessage")]
    public ActionResult<string> SendRabbitMessage()
    {
        var msg = new RabbitMessage("I'm a rabbit");
        rabbitTemplate.ConvertAndSend(Queues.InferredRabbitQueue, msg);
        logger.LogInformation("SendRabbitMessage: sent message to {Queue}", Queues.InferredRabbitQueue);
        return "RabbitMessage sent ... look at logs to see if message processed by a RabbitListener";
    }

    [HttpGet("SendLongEaredRabbitMessage")]
    public ActionResult<string> SendLongEaredRabbitMessage()
    {
        var msg = new LongEaredRabbitMessage("I have long ears");
        rabbitTemplate.ConvertAndSend(Queues.InferredLongEaredRabbitQueue, msg);
        logger.LogInformation("SendLongEaredRabbitMessage: sent message to {Queue}",
            Queues.InferredLongEaredRabbitQueue);
        return "LongEaredRabbitMessage sent ... look at logs to see if message processed by a RabbitListener";
    }

    [HttpGet("SendReceiveRabbitMessage")]
    public ActionResult<string> SendReceiveRabbitMessage()
    {
        var msg = new RabbitMessage("hopping to and fro");
        rabbitTemplate.ConvertAndSend(Queues.SendReceiveRabbitQueue, msg);
        logger.LogInformation("SendReceiveRabbitMessage: sent \"{Message}\" -> {Queue}", msg,
            Queues.SendReceiveRabbitQueue);
        msg = rabbitTemplate.ReceiveAndConvert<RabbitMessage>(Queues.SendReceiveRabbitQueue, 10_000);
        logger.LogInformation("SendReceiveRabbitMessage: received \"{Message}\" <- {Queue}", msg,
            Queues.SendReceiveRabbitQueue);
        return msg.ToString();
    }

    [HttpGet("SendReceiveLongEaredRabbitMessage")]
    public ActionResult<string> SendReceiveLongEaredRabbitMessage()
    {
        var msg = new LongEaredRabbitMessage("flopping my ears to and fro");
        rabbitTemplate.ConvertAndSend(Queues.SendReceiveRabbitQueue, msg);
        logger.LogInformation("SendReceiveLongEaredRabbitMessage: sent \"{Message}\" -> {Queue}", msg,
            Queues.SendReceiveRabbitQueue);
        msg = rabbitTemplate.ReceiveAndConvert<LongEaredRabbitMessage>(Queues.SendReceiveRabbitQueue, 10_000);
        logger.LogInformation("SendReceiveLongEaredRabbitMessage: received \"{Message}\" <- {Queue}", msg,
            Queues.SendReceiveRabbitQueue);
        return msg.ToString();
    }

    [HttpGet("DeleteQueues")]
    public ActionResult<string> DeleteQueues()
    {
        rabbitAdmin.DeleteQueue(Queues.SendReceiveRabbitQueue);
        logger.LogInformation("Deleted queue {Queue}", Queues.SendReceiveRabbitQueue);
        return ("Delete queue complete\n ... All done!");
    }

    [HttpGet("QuorumQueue")]
    public ActionResult<string> QuorumQueue()
    {
        var msg = new RabbitMessage("hopping to and fro, quorum ");
        rabbitTemplate.ConvertAndSend(Queues.QuorumQueue, msg);
        logger.LogInformation("Sent to QuorumQueue: \"{Message}\" -> {Queue}", msg, Queues.QuorumQueue);
        msg = rabbitTemplate.ReceiveAndConvert<RabbitMessage>(Queues.QuorumQueue, 10_000);
        logger.LogInformation("Receive from QuorumQueue: \"{Message}\" <- {Queue}", msg, Queues.QuorumQueue);
        return msg.ToString();
    }
}
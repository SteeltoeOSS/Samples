using DynamicDestinationMessaging.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging;
using Steeltoe.Stream.Binding;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDestinationMessaging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly BinderAwareChannelResolver binderAwareChannelResolver;
        private readonly ILogger<MessageController> logger;

        public MessageController(
            BinderAwareChannelResolver binderAwareChannelResolver,
            ILogger<MessageController> logger)
        {
            this.binderAwareChannelResolver = binderAwareChannelResolver;
            this.logger = logger;
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<string> PostMessage([FromBody] SampleMessage message)
        {
            logger.LogDebug($"preparing message for {message.Destination}");

            var messageChannel = binderAwareChannelResolver.ResolveDestination(message.Destination);

            logger.LogDebug($"retrieved message channel {messageChannel.ServiceName}");

            var streamMessage = Message.Create(Encoding.UTF8.GetBytes(message.Body));

            logger.LogDebug($"stream message created from input");

            var messageWasSent = await messageChannel.SendAsync(streamMessage);

            var messageStatus = messageWasSent ? "SUCCESS" : "FAILURE";

            logger.LogDebug($"Status: {messageStatus}; Service: {messageChannel.ServiceName}");

            return $"Message {messageStatus} for {messageChannel.ServiceName}: {message}";
        }
    }
}

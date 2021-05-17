using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging;
using Steeltoe.Stream.Binding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public async Task<string> PostMessage()
        {
            var message = "This is a test";
            var destination = "steeltoestream.steeltoebasicprocessor";

            logger.LogDebug($"preparing message for {destination}");

            var messageChannel = binderAwareChannelResolver.ResolveDestination("steeltoestream.steeltoebasicprocessor");

            logger.LogDebug($"retrieved message channel {messageChannel.ServiceName}");

            var streamMessage = Message.Create<byte[]>(Encoding.UTF8.GetBytes(message));

            logger.LogDebug($"stream message created from input");

            var messageWasSent = await messageChannel.SendAsync(streamMessage);

            var messageStatus = messageWasSent ? "SUCCESS" : "FAILURE";

            logger.LogDebug($"Status: {messageStatus}; Service: {messageChannel.ServiceName}");

            return $"The following message was sent to {messageChannel.ServiceName}: {message}";
        }
    }
}

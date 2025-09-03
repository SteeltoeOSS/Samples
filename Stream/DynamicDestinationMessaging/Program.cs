using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Messaging;
using Steeltoe.Messaging.Core;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Binding;
using Steeltoe.Stream.Messaging;
using Steeltoe.Stream.StreamHost;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDestinationMessaging
{
    [EnableBinding(typeof(ISink))]
    class Program
    {
        private static BinderAwareChannelResolver binderAwareChannelResolver;
        private static ILogger<Program> logger;

        static async Task Main(string[] args)
        {
            var host = StreamHost.CreateDefaultBuilder<Program>(args).Build();

            binderAwareChannelResolver =
                host.Services.GetService<IDestinationResolver<IMessageChannel>>() as BinderAwareChannelResolver;

            logger = host.Services.GetService<ILogger<Program>>();

            await host.RunAsync();
        }

        [StreamListener(ISink.INPUT)]
        public async void Handle(string message)
        {
            logger.LogTrace("Received message '{Message}', determining destination...", message);
            var destination = GetDestination(message);
            logger.LogTrace("Preparing message for destination {Destination}...", destination);
            var messageChannel = binderAwareChannelResolver.ResolveDestination(destination);
            logger.LogTrace("Retrieved message channel {ServiceName}", messageChannel.ServiceName);
            var streamMessage = Message.Create(Encoding.UTF8.GetBytes(message));
            logger.LogTrace("Stream message created from input.");

            var messageWasSent = await messageChannel.SendAsync(streamMessage);
            var messageStatus = messageWasSent ? "SUCCESS" : "FAILURE";
            logger.LogDebug("Status: {MessageStatus}; Service: {ServiceName}", messageStatus, messageChannel.ServiceName);
        }

        private static string GetDestination(string message)
        {
            return message switch
            {
                not null when message.Contains("customer") => "steeltoestream.customerrequest",
                not null when message.Contains("developer") => "steeltoestream.developerrequest",
                _ => "steeltoestream.generalrequest"
            };
        }
    }
}

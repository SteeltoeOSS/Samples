using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
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
    [EnableBinding(typeof(IProcessor))]
    class Program
    {
        private static BinderAwareChannelResolver binderAwareChannelResolver;
        private static ILogger<Program> logger;

        static async Task Main(string[] args)
        {
            var host = StreamHost.CreateDefaultBuilder<Program>(args)
                .ConfigureAppConfiguration(config => {
                    config.AddJsonFile("appsettings.json");
                })
                .Build();

            binderAwareChannelResolver =
                host.Services.GetService<IDestinationResolver<IMessageChannel>>() as BinderAwareChannelResolver;

            logger = host.Services.GetService<ILogger<Program>>();

            await host.StartAsync();
        }

        [StreamListener(IProcessor.INPUT)]
        public async void Handle(string message)
        {
            logger.LogDebug($"received message '{message}' and determining destination");

            var destination = GetDestination(message);

            logger.LogDebug($"preparing message for destination {destination}");

            var messageChannel = binderAwareChannelResolver.ResolveDestination(destination);

            logger.LogDebug($"retrieved message channel {messageChannel.ServiceName}");

            var streamMessage = Message.Create(Encoding.UTF8.GetBytes(message));

            logger.LogDebug($"stream message created from input");

            var messageWasSent = await messageChannel.SendAsync(streamMessage);

            var messageStatus = messageWasSent ? "SUCCESS" : "FAILURE";

            logger.LogDebug($"Status: {messageStatus}; Service: {messageChannel.ServiceName}");
        }

        private static string GetDestination(string message)
        {
            return message switch
            {
                string customer when customer.Contains("customer") => "steeltoestream.customerrequest",
                string developer when developer.Contains("developer") => "steeltoestream.developerrequest",
                string general when general.Contains("general") => "steeltoestream.generalrequest",
                _ => "steeltoestream.generalrequest"
            };
        }
    }
}

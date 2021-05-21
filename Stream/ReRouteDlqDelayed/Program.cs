using Steeltoe.Messaging;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Messaging.RabbitMQ.Attributes;
using Steeltoe.Messaging.RabbitMQ.Core;
using Steeltoe.Messaging.RabbitMQ.Exceptions;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Messaging.Support;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using Steeltoe.Stream.StreamHost;
using System.Text;
using System.Threading.Tasks;

namespace ReRouteDlqDelayed
{
    public class Program
    {
        private const string ORIGINAL_QUEUE = "so8400in.so8400";
        private const string DLQ = ORIGINAL_QUEUE + ".dlq";
        private const string PARKING_LOT = ORIGINAL_QUEUE + ".parkingLot";
        private const string X_RETRIES_HEADER = "x-retries";
        private const string DELAY_EXCHANGE = "dlqReRouter";

        static async Task Main(string[] args)
        {
            var host = StreamHost
              .CreateDefaultBuilder<ReRouteDlq>(args)
              .ConfigureServices((ctx, services) =>
              {
                  // Add steeltoe rabbit services for RabbitListener and RabbitTemplate
                  services.AddRabbitServices();
                  services.AddRabbitTemplate();

                  // Tell steeltoe about singleton so it can wire up queues with methods to process queues (i.e. RabbitListenerAttribute)
                  services.AddRabbitListeners<ReRouteDlq>();
              })
              .Build();

            await host.StartAsync();
        }

        [EnableBinding(typeof(ISink))]
        public class ReRouteDlq
        {
            private readonly RabbitTemplate rabbitTemplate;

            public ReRouteDlq(RabbitTemplate template)
            {
                rabbitTemplate = template;
            }

            [DeclareQueue(Name = PARKING_LOT)]
            [DeclareExchange(Name = "delayExchange", Delayed = "True")]
            [DeclareQueueBinding(Name = "bindOriginalToDelay", QueueName = ORIGINAL_QUEUE, ExchangeName = "delayExchange")]
            [RabbitListener(DLQ)]
            public void RePublish(
                string text,
                [Header(Name = X_RETRIES_HEADER, Required = false)]
                int? retriesHeader)
            {
                var failedMessage = MessageBuilder
                    .WithPayload(Encoding.UTF8.GetBytes(text))
                    .SetHeader(X_RETRIES_HEADER, (retriesHeader ?? 0) + 1)
                    .SetHeader("x-delay", 5000*retriesHeader)
                    .Build();

                if (!retriesHeader.HasValue || retriesHeader < 3)
                {
                    rabbitTemplate.Send(ORIGINAL_QUEUE, failedMessage);
                }
                else
                {
                    rabbitTemplate.Send(PARKING_LOT, failedMessage);
                }
            }

            [StreamListener(ISink.INPUT)]
            public void InitialMessage(IMessage failedMessage)
            {
                throw new RabbitRejectAndDontRequeueException("failed");
            }
        }

    }
}

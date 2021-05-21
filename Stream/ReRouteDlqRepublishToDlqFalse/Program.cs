using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Messaging.RabbitMQ.Attributes;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Core;
using Steeltoe.Messaging.RabbitMQ.Exceptions;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Messaging.Support;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using Steeltoe.Stream.StreamHost;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReRouteDlqRepublishToDlqFalse
{
    public class Program
    {
        private const string ORIGINAL_QUEUE = "so8400in.so8400";
        private const string DLQ = ORIGINAL_QUEUE + ".dlq";
        private const string PARKING_LOT = ORIGINAL_QUEUE + ".parkingLot";
        private const string X_DEATH_HEADER = "x-death";
        private const string X_RETRIES_HEADER = "x-retries";

        static async Task Main(string[] args)
        {
            var host = StreamHost
              .CreateDefaultBuilder<ReRouteDlq>(args)
              .ConfigureServices((ctx, services) =>
              {
                  services.AddRabbitServices();
                  services.AddRabbitTemplate();

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
            [RabbitListener(DLQ)]
            public void RePublish(
                string text,
                [Header(Name = X_RETRIES_HEADER, Required = false)]
                int? retriesHeader,
                [Header(Name = X_DEATH_HEADER, Required = false)]
                IDictionary<string, object> xDeathHeader
                )
            {
                var failedMessage = MessageBuilder.WithPayload(Encoding.UTF8.GetBytes(text)).SetHeader(X_RETRIES_HEADER, (retriesHeader ?? 0) + 1).Build();

                if (!retriesHeader.HasValue || retriesHeader < 3)
                {
                    if (xDeathHeader != null
                        && xDeathHeader.TryGetValue("exchange", out var exchange)
                        && exchange is string strExchange
                        && xDeathHeader.TryGetValue("routing-keys", out var rk)
                        && rk is List<object> routingKeys)
                    {
                        rabbitTemplate.Send(strExchange, routingKeys[0].ToString(), failedMessage);
                    }
                    else
                    {
                        throw new RabbitRejectAndDontRequeueException("failed");
                    }    
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

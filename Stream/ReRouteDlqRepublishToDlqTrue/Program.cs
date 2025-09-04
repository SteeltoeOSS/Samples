﻿using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Messaging.RabbitMQ.Attributes;
using Steeltoe.Messaging.RabbitMQ.Core;
using Steeltoe.Messaging.RabbitMQ.Exceptions;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using Steeltoe.Messaging.RabbitMQ.Retry;
using Steeltoe.Messaging.Support;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using Steeltoe.Stream.StreamHost;
using System.Text;
using System.Threading.Tasks;

namespace ReRouteDlqRepublishToDlqTrue;

public class Program
{
    private const string ORIGINAL_QUEUE = "so8400in.so8400";
    private const string DLQ = ORIGINAL_QUEUE + ".dlq";
    private const string PARKING_LOT = ORIGINAL_QUEUE + ".parkingLot";
    private const string X_RETRIES_HEADER = "x-retries";
    private const string X_ORIGINAL_EXCHANGE_HEADER = RepublishMessageRecoverer.X_ORIGINAL_EXCHANGE;
    private const string X_ORIGINAL_ROUTING_KEY_HEADER = RepublishMessageRecoverer.X_ORIGINAL_ROUTING_KEY;

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

        await host.RunAsync();
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
            [Header(Name = X_ORIGINAL_EXCHANGE_HEADER, Required = false)]
            string exchange,
            [Header(Name = X_ORIGINAL_ROUTING_KEY_HEADER, Required = false)]
            string originalRoutingKey
        )
        {
            var failedMessage = MessageBuilder
                .WithPayload(Encoding.UTF8.GetBytes(text))
                .SetHeader(X_RETRIES_HEADER, (retriesHeader ?? 0) + 1)
                .Build();

            if (!retriesHeader.HasValue || retriesHeader < 3)
            {
                if (exchange != null && !string.IsNullOrEmpty(originalRoutingKey) )
                {
                    rabbitTemplate.Send(exchange, originalRoutingKey, failedMessage);
                }
                else
                {
                    rabbitTemplate.Send(ORIGINAL_QUEUE, failedMessage);
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
            throw new RabbitRejectAndDontRequeueException($"Intentionally failed to process: {failedMessage.Payload}");
        }
    }
}
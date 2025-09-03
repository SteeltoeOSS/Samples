# Partitioned Producer Sample

This sample must be paired with [PartitionedConsumer](../PartitionedConsumer/README.md) to demonstrate partitioned messaging.]

## Pre-requisites

1. .NET SDK
1. RabbitMQ Server (see [CommonTasks](../../CommonTasks.md#rabbitmq))

## Running the sample

In a command line console, change into the project root directory. Run the project and observe the log output.

```bash
cd PartitionedProducer
dotnet run 

info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\source\SteeltoeOSS\Samples\Stream\PartitionedProducer
info: Steeltoe.Messaging.RabbitMQ.Connection.CachingConnectionFactory[0]
      Attempting to connect to: amqp://127.0.0.1:5672
info: Steeltoe.Messaging.RabbitMQ.Connection.CachingConnectionFactory[0]
      Created new connection: ccFactory:1.publisher/Steeltoe.Messaging.RabbitMQ.Connection.SimpleConnection
info: Steeltoe.Stream.Binder.Rabbit.RabbitMessageChannelBinder[0]
      Channel 'input.anonymous.SZCF6dK6WEOqhG70h3HLKQ.errors' has 1 subscriber(s).
info: Steeltoe.Stream.Binder.Rabbit.RabbitMessageChannelBinder[0]
      Channel 'input.anonymous.SZCF6dK6WEOqhG70h3HLKQ.errors' has 2 subscriber(s).
info: PartitionedProducer.Worker[0]
      Worker running at: 09/03/2025 14:45:49 -05:00
Sending: def3
info: Steeltoe.Messaging.RabbitMQ.Connection.CachingConnectionFactory[0]
      Attempting to connect to: amqp://127.0.0.1:5672
info: Steeltoe.Messaging.RabbitMQ.Connection.CachingConnectionFactory[0]
      Created new connection: ccFactory:1.publisher/Steeltoe.Messaging.RabbitMQ.Connection.SimpleConnection
info: Steeltoe.Stream.Binder.Rabbit.Provisioning.RabbitExchangeQueueProvisioner[0]
      Auto-declaring a non-durable, auto-delete, or exclusive Queue (input.anonymous.SZCF6dK6WEOqhG70h3HLKQ) durable:False, auto-delete:True, exclusive:True.It will be redeclared if the broker stops and is restarted while the connection factory is alive, but all messages will be lost.
Sending: qux3
info: PartitionedProducer.Worker[0]
      Worker running at: 09/03/2025 14:45:54 -05:00
info: PartitionedProducer.Worker[0]
      Worker running at: 09/03/2025 14:45:59 -05:00
Sending: def2
Sending: def4
```

With both the producer and consumer running, you will see messages being sent by the producer, but only some of them will be received by the consumer.
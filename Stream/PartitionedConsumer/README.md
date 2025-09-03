# Partitioned Consumer Sample

This sample must be paired with [PartitionedProducer](../PartitionedProducer/README.md) to demonstrate partitioned messaging.]

## Pre-requisites

1. .NET SDK
1. RabbitMQ Server (see [CommonTasks](../../CommonTasks.md#rabbitmq))

## Running the sample

In a command line console, change into the project root directory. Run the project.

```bash
cd PartitionedConsumer
dotnet run 

info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\source\SteeltoeOSS\Samples\Stream\PartitionedConsumer
info: Steeltoe.Messaging.RabbitMQ.Connection.CachingConnectionFactory[0]
      Attempting to connect to: amqp://127.0.0.1:5672
info: Steeltoe.Messaging.RabbitMQ.Connection.CachingConnectionFactory[0]
      Created new connection: ccFactory:1.publisher/Steeltoe.Messaging.RabbitMQ.Connection.SimpleConnection
info: Steeltoe.Stream.Binder.Rabbit.RabbitMessageChannelBinder[0]
      Channel 'partitioned.destination.myGroup-0.errors' has 1 subscriber(s).
info: Steeltoe.Stream.Binder.Rabbit.RabbitMessageChannelBinder[0]
      Channel 'partitioned.destination.myGroup-0.errors' has 2 subscriber(s).
```

With the consumer running, follow the instructions in the [PartitionedProducer](../PartitionedProducer/README.md) to send messages.

When messages are received, you will see log messages similar to this:

```bash
def3 received from queue partitioned.destination.myGroup-0
def2 received from queue partitioned.destination.myGroup-0
def1 received from queue partitioned.destination.myGroup-0
abc3 received from queue partitioned.destination.myGroup-0
```

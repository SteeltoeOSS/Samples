# Stream Sample using Entity Framework Core

## Pre-requisites

1. .NET SDK
1. RabbitMQ Server (see [CommonTasks](../../CommonTasks.md#rabbitmq))
1. MySQL Server (see [CommonTasks](../../CommonTasks.md#mysql))

## Running the sample

In a command line console, change into the project root directory. Run the project.

```bash
cd EFCore
dotnet run 

info: Steeltoe.Messaging.RabbitMQ.Connection.CachingConnectionFactory[0]
      Attempting to connect to: amqp://127.0.0.1:5672
info: Steeltoe.Messaging.RabbitMQ.Connection.CachingConnectionFactory[0]
      Created new connection: ccFactory:1.publisher/Steeltoe.Messaging.RabbitMQ.Connection.SimpleConnection
info: Steeltoe.Stream.Binder.Rabbit.RabbitMessageChannelBinder[0]
      Channel 'sample-sink-data.anonymous.UvdH4-vUaUu8boI1FNW3wg.errors' has 1 subscriber(s).
info: Steeltoe.Stream.Binder.Rabbit.RabbitMessageChannelBinder[0]
      Channel 'sample-sink-data.anonymous.UvdH4-vUaUu8boI1FNW3wg.errors' has 2 subscriber(s).
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\source\SteeltoeOSS\Samples\Stream\EFCore
info: EFCore.BindableChannels[0]
      Received foo named 'test' with tag 'tag1'
info: EFCore.BindableChannels[0]
      Foo was assigned id '1' after saving to the database.
```

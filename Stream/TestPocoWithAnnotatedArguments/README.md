# POCO with Annotated Arguments Consumer

This sample demonstrates a simple consumer application that receives messages from a RabbitMQ broker.
The consumer is implemented as a plain old CLR object (POCO) with annotated arguments.

## Pre-requisites

1. .NET SDK
1. RabbitMQ Server (see [CommonTasks](../../CommonTasks.md#rabbitmq))

## Running the sample

In a command line console, change into the project root directory. Run the project.

```bash
cd TestPocoWithAnnotatedArguments
dotnet run 

info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\source\SteeltoeOSS\Samples\Stream\TestPocoWithAnnotatedArguments
info: Steeltoe.Messaging.RabbitMQ.Connection.CachingConnectionFactory[0]
      Attempting to connect to: amqp://127.0.0.1:5672
info: Steeltoe.Messaging.RabbitMQ.Connection.CachingConnectionFactory[0]
      Created new connection: ccFactory:1.publisher/Steeltoe.Messaging.RabbitMQ.Connection.SimpleConnection
info: Steeltoe.Stream.Binder.Rabbit.RabbitMessageChannelBinder[0]
      Channel 'input.anonymous.FMyrUMu6k0OHpNKFINWC4Q.errors' has 1 subscriber(s).
info: Steeltoe.Stream.Binder.Rabbit.RabbitMessageChannelBinder[0]
      Channel 'input.anonymous.FMyrUMu6k0OHpNKFINWC4Q.errors' has 2 subscriber(s).
```

1. Go to your [RabbitMQ broker queues](http://localhost:15672/#/queues). Find a queue named like `input.anonymous.<some-random-string>` and click on it (this name is generated at runtime and will vary).
1. Find the `Publish Message` section of the UI
1. Under `Headers`, set `type` = `Dog` (values are case sensitive) 
1. Under `Payload`, enter a JSON object that represents an instance of the Dog class:
   ```json
    { "Bark": "woof" }
   ```
1. Click `Publish Message`

You will see a log message similar to this:

```bash
Dog says:woof
```

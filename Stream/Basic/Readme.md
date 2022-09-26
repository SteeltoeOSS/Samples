# Basic Stream Sample using WebApplication Builder

This sample assumes you have rabbitMQ broker running with default credentials. 

# Running the sample

In a command line console, change into the project root directory. Run the project.

```bash
cd Basic
dotnet run 


info: Steeltoe.Messaging.RabbitMQ.Connection.CachingConnectionFactory[0]
      Attempting to connect to: amqp://127.0.0.1:5672
info: Steeltoe.Messaging.RabbitMQ.Connection.CachingConnectionFactory[0]
      Created new connection: ccFactory:1.publisher/Steeltoe.Messaging.RabbitMQ.Connection.SimpleConnection
info: Steeltoe.Stream.Binder.Rabbit.RabbitMessageChannelBinder[0]
      Channel 'BasicSample.Basic.errors' has 1 subscriber(s).
info: Steeltoe.Stream.Binder.Rabbit.RabbitMessageChannelBinder[0]
      Channel 'BasicSample.Basic.errors' has 2 subscriber(s).
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
      ...
```

Go to your RabbitMQ broker queues:
You should see a queue named: `BasicSample.Basic`. This name is controlled by settings in the `appsettings.json`.

Type a string into the `Publish Message` section of the UI, and hit `Publish Message`

You will see a log message similar to this:

```bash
MyStreamProcssor changed input:test into output:TEST
```

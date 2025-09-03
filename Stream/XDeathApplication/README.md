# XDeathApplication

## Pre-requisites

1. .NET SDK
1. RabbitMQ Server (see [CommonTasks](../../CommonTasks.md#rabbitmq))

## Running the sample

In a command line console, change into the project root directory. Run the project.

```bash
cd XDeathApplication
dotnet run 

info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\source\SteeltoeOSS\Samples\Stream\XDeathApplication
info: Steeltoe.Messaging.RabbitMQ.Connection.CachingConnectionFactory[0]
      Attempting to connect to: amqp://127.0.0.1:5672
info: Steeltoe.Messaging.RabbitMQ.Connection.CachingConnectionFactory[0]
      Created new connection: ccFactory:1.publisher/Steeltoe.Messaging.RabbitMQ.Connection.SimpleConnection
info: Steeltoe.Stream.Binder.Rabbit.RabbitMessageChannelBinder[0]
      Channel 'myDestination2.consumerGroup.errors' has 1 subscriber(s).
info: Steeltoe.Stream.Binder.Rabbit.RabbitMessageChannelBinder[0]
      Channel 'myDestination2.consumerGroup.errors' has 2 subscriber(s).
```

1. Go to your [RabbitMQ broker queues](http://localhost:15672/#/queues). Find the queue named `myDestination2.consumerGroup` and click on it (this name is controlled by settings in the `appsettings.json`).
1. Find the `Publish Message` section of the UI and enter any text in the `Payload` field. For example: `Some message`
1. Click `Publish Message`

You will see four log messages similar to this:

```bash
fail: Steeltoe.Integration.Handler.LoggingHandler[0]
      Steeltoe.Messaging.MessagingException: Exception thrown while invoking Program#Listen[2 args]
       ---> Steeltoe.Messaging.RabbitMQ.Exceptions.RabbitRejectAndDontRequeueException: failed
         at Steeltoe.Messaging.Handler.Invocation.InvocableHandlerMethod.DoInvoke(Object[] args)
         at Steeltoe.Messaging.Handler.Invocation.InvocableHandlerMethod.Invoke(IMessage requestMessage, Object[] args)
         at Steeltoe.Stream.Binding.StreamListenerMessageHandler.HandleRequestMessage(IMessage requestMessage)
         --- End of inner exception stack trace ---
         at Steeltoe.Stream.Binding.StreamListenerMessageHandler.HandleRequestMessage(IMessage requestMessage)
         at Steeltoe.Integration.Handler.AbstractReplyProducingMessageHandler.HandleMessageInternal(IMessage message)
         at Steeltoe.Integration.Handler.AbstractMessageHandler.HandleMessage(IMessage message)
         at Steeltoe.Integration.Dispatcher.AbstractDispatcher.TryOptimizedDispatch(IMessage message)
         at Steeltoe.Integration.Dispatcher.UnicastingDispatcher.DoDispatch(IMessage message, CancellationToken cancellationToken)
         at Steeltoe.Integration.Dispatcher.UnicastingDispatcher.Dispatch(IMessage message, CancellationToken cancellationToken)
         at Steeltoe.Integration.Channel.AbstractSubscribableChannel.DoSendInternal(IMessage message, CancellationToken cancellationToken)
         at Steeltoe.Integration.Channel.AbstractMessageChannel.DoSend(IMessage message, CancellationToken cancellationToken)
         at Steeltoe.Integration.Channel.AbstractMessageChannel.DoSend(IMessage message, Int32 timeout)
         at Steeltoe.Integration.Channel.AbstractMessageChannel.Send(IMessage message, Int32 timeout)
         at Steeltoe.Messaging.Core.MessageChannelTemplate.DoSend(IMessageChannel channel, IMessage message, Int32 timeout)
         at Steeltoe.Messaging.Core.MessageChannelTemplate.DoSend(IMessageChannel destination, IMessage message)
         at Steeltoe.Messaging.Core.AbstractMessageSendingTemplate`1.Send(D destination, IMessage message)
         at Steeltoe.Integration.Endpoint.MessageProducerSupportEndpoint.SendMessage(IMessage messageArg), failedMessage=Message`1 [payload=byte[13], headers=System.Collections.Generic.Dictionary`2[System.String,System.Object]]
```

After the four failures, the message will disappear.
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Connection;
using Steeltoe.Messaging.RabbitMQ.Core;
using System;

var connectionFactory = new CachingConnectionFactory { Host = "localhost" };
var admin = new RabbitAdmin(connectionFactory);
admin.DeclareQueue(new Queue("myQueue"));
var template = new RabbitTemplate(connectionFactory);
template.ConvertAndSend("myQueue", "foo");
var foo = template.ReceiveAndConvert<string>("myQueue");
admin.DeleteQueue("myQueue");
connectionFactory.Dispose();
Console.WriteLine("Received message: {0}", foo);

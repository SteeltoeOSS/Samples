using System;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Connection;
using Steeltoe.Messaging.RabbitMQ.Core;

namespace ConsoleSendReceive
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionFactory = new CachingConnectionFactory()
            {
                Host = "localhost"
            };
            var admin = new RabbitAdmin(connectionFactory); 
            admin.DeclareQueue(new Queue("myqueue"));
            var template = new RabbitTemplate(connectionFactory);
            template.ConvertAndSend("myqueue", "foo");
            var foo = template.ReceiveAndConvert<string>("myqueue");
            admin.DeleteQueue("myQueue");
            connectionFactory.Dispose();
            Console.WriteLine(foo);
        }
    }
}

using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MonitorRabbitMQ;
using Steeltoe.Connector.RabbitMQ;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure RabbitMQ with Steeltoe Messaging
builder.Services.AddRabbitMQConnection(builder.Configuration);

// Add Steeltoe Rabbit services, use default .NET serialization
//builder.Services.AddRabbitServices();

// Add Steeltoe Rabbit services, use JSON serialization
builder.Services.AddRabbitServices(true);

// Add Steeltoe RabbitAdmin services to get queues declared
builder.Services.AddRabbitAdmin();

// Add a queue to the message container that the rabbit admin will discover and declare at startup
builder.Services.AddRabbitQueue(new Queue(Constants.ReceiveAndConvertQueue));

// Add singleton that will process incoming messages
builder.Services.AddSingleton<RabbitListenerService>();

// Tell steeltoe about singleton so it can wire up queues with methods to process queues
builder.Services.AddRabbitListeners<RabbitListenerService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/", () => "Monitoring RabbitMQ, but there's nothing to do from the web interface - watch the logs!");

app.Run();
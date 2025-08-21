using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Steeltoe.Connector.RabbitMQ;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "WriteToRabbitMQ", Version = "v1" }));

// Configure RabbitMQ with Steeltoe Messaging
builder.Services.AddRabbitMQConnection(builder.Configuration);

// Add Steeltoe Rabbit services, use default .NET serialization
//builder.Services.AddRabbitServices();

// Add Steeltoe Rabbit services, use JSON serialization
builder.Services.AddRabbitServices(true);

// Add Steeltoe RabbitAdmin services to get queues declared
builder.Services.AddRabbitAdmin();

// Add Steeltoe RabbitTemplate for sending/receiving
builder.Services.AddRabbitTemplate();

// Add a queue to the message container that the rabbit admin will discover and declare at startup
builder.Services.AddRabbitQueue(new Queue(Constants.ReceiveAndConvertQueue));

builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WriteToRabbitMQ"));
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
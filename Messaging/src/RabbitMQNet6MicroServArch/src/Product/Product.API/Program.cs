using Common.SeedWork;
using Product.API.Models.Requests;
using Steeltoe.Connector.RabbitMQ;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Core;
using Steeltoe.Messaging.RabbitMQ.Extensions;

var builder = WebApplication.CreateBuilder(args);
// Build a config object, using env vars and JSON providers.
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.Development.json")
    .AddEnvironmentVariables()
    .Build();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region SteelToeRabbitMQ Configuration
//when you call ConfigureRabbitOptions method then appsettings.env.json specified connection configuration will be applied otherwise
//if you dont call that method then default settings will be picked and even you dont need to specify any configuration in appsettings
builder.Services.ConfigureRabbitOptions(config);
// Add Steeltoe Rabbit services, use JSON serialization
builder.Services.AddRabbitServices(true);

// Add Steeltoe RabbitAdmin services to get queues declared
builder.Services.AddRabbitAdmin();

// Add Steeltoe RabbitTemplate for sending/receiving
builder.Services.AddRabbitTemplate();

// Add a queue to the message container that the rabbit admin will discover and declare at startup
builder.Services.AddRabbitQueue(new Queue(Queues.ProductAddQueue));
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/product", async (AddProductRequestDTO request, RabbitTemplate _rabbitTemplate, RabbitAdmin _rabbitAdmin
    ,CancellationToken cancellationToken) =>
{
    var msg = new Message() {Type="Information", Body = "Hi there from over here." };

    await _rabbitTemplate.ConvertAndSendAsync(Queues.ProductAddQueue, msg, cancellationToken);

    return Results.Ok("Product Added Successfully");
})
.WithName("GetWeatherForecast");
app.Run();
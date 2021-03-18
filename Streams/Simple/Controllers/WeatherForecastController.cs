using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Steeltoe.Common.Util;
using Steeltoe.Messaging;
using Steeltoe.Messaging.Support;
using Steeltoe.Stream.Messaging;

namespace steeltoe_stream_samples.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase//, IMessageHandler
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly SomeClass someClass;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, SomeClass someClass)
        {
            _logger = logger;
            this.someClass = someClass;
        }

        public string ServiceName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            var forecast =  Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
            
          
            return forecast;
        }
        [HttpGet("sendoutput")]
        public void Output()
        {
            // _source.Output.Send(MessageBuilder.WithPayload("{\"name\":\"output\"}").Build());
            someClass.Echo(EncodingUtils.Utf8.GetBytes("FooBar"));//.Name
        }
        [HttpGet("sendinput")]
        public void SendInput()
        {
           // _sink.Input.Send(MessageBuilder.WithPayload("{\"name\":\"input\"}").Build());
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void HandleMessage(IMessage message)
        {
            Console.WriteLine(message.Payload);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Steeltoe.Common.Util;
using Steeltoe.Connector;
using Steeltoe.Connector.Services;
using Steeltoe.Messaging;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Connection;
using Steeltoe.Messaging.Support;
using Steeltoe.Stream.Binder.Rabbit.Config;
using Steeltoe.Stream.Config;
using Steeltoe.Stream.Messaging;

namespace CloudDataflowToUpperProcessor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly Program p;
        private readonly IOptionsMonitor<BindingServiceOptions> bindingsOptions;
        private readonly Steeltoe.Messaging.RabbitMQ.Connection.IConnectionFactory connectionFactory;
        private readonly IOptionsMonitor<RabbitOptions> rabbitOptions;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, Program p, IOptionsMonitor<BindingServiceOptions> bindingsOptions, Steeltoe.Messaging.RabbitMQ.Connection.IConnectionFactory connectionFactory, IOptionsMonitor<RabbitOptions> rabbitOptions)
        {
            _logger = logger;
            this.p = p;
            this.bindingsOptions = bindingsOptions;
            this.connectionFactory = connectionFactory;
            this.rabbitOptions = rabbitOptions;
        }

  
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            p.Handle("test 123");
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
        [HttpGet("Config")]
        public BindingServiceOptions GetConfig()
        {
            return this.bindingsOptions.CurrentValue ;
        }

        [HttpGet("RabbitOptions")]
        public string GetconnectionFactory()
        {
            var o = rabbitOptions.CurrentValue;
            var cc = connectionFactory as CachingConnectionFactory;
            return $"cc: {cc?.Host}:{cc?.VirtualHost} {cc?.Username}:{cc?.Password}\n"
            + $"{o.Host}:{o.VirtualHost}:{o.Username}:{o.Password}";
        }
    }
}

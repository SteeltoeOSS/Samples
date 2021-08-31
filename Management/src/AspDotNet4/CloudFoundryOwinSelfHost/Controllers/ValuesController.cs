using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;

namespace CloudFoundryOwinSelfHost.Controllers
{
    public class ValuesController : ApiController
    {
        private ILogger<ValuesController> _logger;

        public ValuesController()
        {
            _logger = ApplicationConfig.LoggerFactory.CreateLogger<ValuesController>();
        }

        // GET api/values 
        public IEnumerable<string> Get()
        {
            var minlvl = GetMinLogLevel(_logger);
            Console.WriteLine($"Minimum level set on _logger: {minlvl}");
            Debug.WriteLine($"Minimum level set on _logger: {minlvl}");
            _logger.LogTrace("This is a {LogLevel} log", LogLevel.Trace.ToString());
            _logger.LogDebug("This is a {LogLevel} log", LogLevel.Debug.ToString());
            _logger.LogInformation("This is a {LogLevel} log", LogLevel.Information.ToString());
            _logger.LogWarning("This is a {LogLevel} log", LogLevel.Warning.ToString());
            _logger.LogError("This is a {LogLevel} log", LogLevel.Error.ToString());
            _logger.LogCritical("This is a {LogLevel} log", LogLevel.Critical.ToString());
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5 
        public string Get(int id)
        {
            return "value";
        }


        private LogLevel GetMinLogLevel(ILogger logger)
        {
            for (var i = 0; i < 6; i++)
            {
                var level = (LogLevel)Enum.ToObject(typeof(LogLevel), i);
                if (logger.IsEnabled(level))
                {
                    return level;
                }
            }

            return LogLevel.None;
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Steeltoe.Extensions.Logging;

namespace FortuneTellerUI4
{
    public static class LoggingConfig
    {
        public static ILoggerFactory LoggerFactory { get; set; }
        public static ILoggerProvider LoggerProvider { get; set; }
        public static void Register(IConfiguration configuration)
        {
            LoggerProvider = new DynamicLoggerProvider(new ConsoleLoggerSettings().FromConfiguration(configuration));
            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddProvider(LoggerProvider);
        }
    }
}
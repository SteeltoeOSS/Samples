using System.Collections.Generic;

namespace Steeltoe.Actuators.Models
{
    public class DynamicLogLevels
    {
        public List<string> Levels { get; set; } = new();

        public Dictionary<string, DynamicLogLevel> Loggers { get; set; }  = new();
    }

    public class DynamicLogLevel
    {
        public string ConfiguredLevel { get; set;}
        public string EffectiveLevel { get; set; }
    }
}

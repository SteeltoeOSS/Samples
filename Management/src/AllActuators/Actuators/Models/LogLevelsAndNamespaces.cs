using System.Collections.Generic;

namespace Steeltoe.Actuators.Models
{
    public class LogLevelsAndNamespaces
    {
        public List<string> Levels { get; set; } = new();

        public Dictionary<string, LogNamespace> Loggers { get; set; }  = new();
    }

    public class LogNamespace
    {
        public string ConfiguredLevel { get; set; }

        public string EffectiveLevel { get; set; }
    }
}

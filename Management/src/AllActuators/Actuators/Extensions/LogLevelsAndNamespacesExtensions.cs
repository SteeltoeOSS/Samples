using Steeltoe.Actuators.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Steeltoe.Actuators.Extensions
{
    public static class LogLevelsAndNamespacesExtensions
    {
        public static void Filter(
            this LogLevelsAndNamespaces logLevelsAndNamespaces, 
            string filter, 
            int page = 0,
            int maxCount = 0)
        {
            if (!string.IsNullOrWhiteSpace(filter))
            {
                logLevelsAndNamespaces.Loggers = logLevelsAndNamespaces.Loggers.Where(kvp => kvp.Key.Contains(filter))
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
        }
    }
}

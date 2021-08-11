using Steeltoe.Actuators.Models;
using System.Collections.Generic;
using System.Linq;

namespace Steeltoe.Actuators.Extensions
{
    public static class DynamicLogLevelsExtensions
    {
        public static IEnumerable<LogLevel> ToLogLevels(this DynamicLogLevels dynamicLogLevels) =>
            dynamicLogLevels.Loggers.Select(kvp =>
                    new LogLevel { Namespace = kvp.Key, Level = kvp.Value.EffectiveLevel });


        public static IEnumerable<LogLevel> Filter(this IEnumerable<LogLevel> logLevels, string filter) =>
            (!string.IsNullOrWhiteSpace(filter)) ?
                logLevels.ToList().FindAll(log =>
                    log.Namespace.Contains(filter) || log.Level.Contains(filter)) : logLevels;

        public static IEnumerable<LogLevel> Paginate(this IEnumerable<LogLevel> logLevels, int pageIndex, int pageSize) =>
            logLevels.AsQueryable().Skip((pageIndex - 1) * pageSize).Take(pageSize);
    }
}

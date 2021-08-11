using Steeltoe.Actuators.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Steeltoe.Actuators.Models
{
    public class LoggingViewModel
    {
        public List<string> AvailableLevels => availableLevels;

        public IEnumerable<LogLevel> LogLevels =>
            logLevels.Filter(SearchKeyword).Paginate(PageIndex, PageSize);

        public int PageIndex { get; set; }

        public int PageSize { get; } = 10;

        public string SearchKeyword { get; set; } = "";

        public int TotalPages =>
            (int)Math.Ceiling(logLevels.Filter(SearchKeyword).Count() / (double)PageSize);
        

        private readonly List<string> availableLevels;
        private readonly IEnumerable<LogLevel> logLevels;

        public LoggingViewModel(DynamicLogLevels dynamicLogLevels)
        {
            availableLevels = dynamicLogLevels.Levels;
            logLevels = dynamicLogLevels.ToLogLevels();
        }
    }
}

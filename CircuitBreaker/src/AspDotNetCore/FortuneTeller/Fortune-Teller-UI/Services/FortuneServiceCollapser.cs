using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Steeltoe.CircuitBreaker.Hystrix;
using System.Reactive;
using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Client;

namespace Fortune_Teller_UI.Services
{
    public class FortuneServiceCollapser : HystrixCollapser<List<Fortune>, Fortune, int>, IFortuneServiceCollapser
    {
        ILogger<FortuneServiceCollapser> _logger;
        ILoggerFactory _loggerFactory;
        IFortuneService _fortuneService;

        public FortuneServiceCollapser(IHystrixCollapserOptions options, IFortuneService fortuneService, ILoggerFactory logFactory) : base(options)
        {
            _logger = logFactory.CreateLogger<FortuneServiceCollapser>();
            _loggerFactory = logFactory;
            _fortuneService = fortuneService;
        }

        public virtual int FortuneId { get; set; }

        public override int RequestArgument { get { return FortuneId; } }

        protected override HystrixCommand<List<Fortune>> CreateCommand(ICollection<ICollapsedRequest<Fortune, int>> requests)
        {
            _logger.LogInformation("Creating MultiFortuneServiceCommand to handle {0} number of requests", requests.Count);
            return new MultiFortuneServiceCommand(
                HystrixCommandGroupKeyDefault.AsKey("MultiFortuneService"), 
                requests,
                _fortuneService,
                _loggerFactory.CreateLogger<MultiFortuneServiceCommand>());
        }

        protected override void MapResponseToRequests(List<Fortune> batchResponse, ICollection<ICollapsedRequest<Fortune, int>> requests)
        {
            foreach(var f in batchResponse)
            {
                foreach(var r in requests)
                {
                    if (r.Argument == f.Id)
                    {
                        r.Response = f;
                    }
                }
            }
        }
    }
}

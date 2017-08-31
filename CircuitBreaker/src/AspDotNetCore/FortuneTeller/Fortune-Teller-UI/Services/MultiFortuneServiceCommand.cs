
using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Client;
using Steeltoe.CircuitBreaker.Hystrix;
using System.Collections.Generic;
using System.Net.Http;
using System.Reactive;
using System.Threading.Tasks;

namespace Fortune_Teller_UI.Services
{
    public class MultiFortuneServiceCommand : HystrixCommand<List<Fortune>>
    {
        ILogger<MultiFortuneServiceCommand> _logger;
        ICollection<ICollapsedRequest<Fortune, int>> _requests;
        IFortuneService _fortuneService;

        public MultiFortuneServiceCommand(IHystrixCommandGroupKey groupKey, 
            ICollection<ICollapsedRequest<Fortune, int>> requests, 
            IFortuneService fortuneService, 
            ILogger<MultiFortuneServiceCommand> logger) : base(groupKey)
        {
            _fortuneService = fortuneService;
            _logger = logger;
            _requests = requests;
        }

        protected override async Task<List<Fortune>> RunAsync()
        {
            List<int> ids = new List<int>();
            foreach(var req in _requests)
            {
                ids.Add(req.Argument);
            }
            return await _fortuneService.GetFortunesAsync(ids);
        }

        protected override async Task<List<Fortune>> RunFallbackAsync()
        {
            List<Fortune> results = new List<Fortune>() { new Fortune() { Id = 9999, Text = "You will have a happy day!" } };
            return await Task.FromResult(results);
        }

  
    }
}

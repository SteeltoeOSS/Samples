using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;

using System.Threading.Tasks;

namespace Fortune_Teller_UI.Services
{
    // Lab09 Start
    public class FortuneServiceCommand : HystrixCommand<Fortune>
    {
        IFortuneService _fortuneService;
        ILogger<FortuneServiceCommand> _logger;
        string fallbackText;

        public FortuneServiceCommand(IHystrixCommandOptions options,
            IFortuneService fortuneService, 
            ILogger<FortuneServiceCommand> logger,
            IConfiguration configuration) : base(options)
        {
            _fortuneService = fortuneService;
            _logger = logger;
            IsFallbackUserDefined = true;
            fallbackText = configuration.GetValue<string>("fallbackFortune");
        }
        public async Task<Fortune> RandomFortuneAsync()
        {
            return await ExecuteAsync();
        }
        protected override async Task<Fortune> RunAsync()
        {
            var result = await _fortuneService.RandomFortuneAsync();
            _logger.LogInformation("Run: {0}", result);
            return result;
        }

        protected override async Task<Fortune> RunFallbackAsync()
        {
            _logger.LogInformation("RunFallback");
            return await Task.FromResult<Fortune>(new Fortune() { Id = 9999, Text = fallbackText });
        }
    }
    // Lab09 End
}

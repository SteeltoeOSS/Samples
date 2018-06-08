using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;
using System.Net.Http;
using System.Threading.Tasks;

namespace Steeltoe.Common.Http
{
    public class HystrixHttpCommandWithRetry : HystrixCommand<HttpResponseMessage>
    {
        private readonly Task<HttpResponseMessage> _baseRequest;
        private readonly ILogger _logger;

        /// <summary>
        /// The most basic circuit breaker with retry ever created!
        /// </summary>
        /// <param name="baseRequest">The HTTP Request to use in the circuit</param>
        /// <param name="logger">An <see cref="ILogger"/></param>
        /// <remarks>If the request fails (or times out) for any reason, the included fallback method immediately retries the operation</remarks>
        public HystrixHttpCommandWithRetry(Task<HttpResponseMessage> baseRequest, ILogger logger = null) : base(HystrixCommandGroupKeyDefault.AsKey("AltRandomFortuneCommand"))
        {
            _baseRequest = baseRequest;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> RunAsync()
        {
            _logger?.LogInformation("Beginning initial request");
            return await _baseRequest;
        }

        protected override async Task<HttpResponseMessage> RunFallbackAsync()
        {
            // TODO: Apply some form of filtering on response code so this isn't a blind retry
            _logger?.LogInformation("Beginning retry request");
            return await _baseRequest;
        }
    }
}

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Steeltoe.CircuitBreaker.Hystrix;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fortune_Teller_UI.Services
{
    /// <summary>
    /// Proof of Concept user-defined hystrix command
    /// </summary>
    public class UserDefinedHystrixCommand : HystrixCommand<HttpResponseMessage>
    {
        private readonly Task<HttpResponseMessage> _baseRequest;
        private readonly ILogger _logger;

        /// <summary>
        /// To use the HystrixHttpMessageHandler with a user-defined type, we need to agree on the constructor definition
        /// and that constructor should include the base request so that the handler has a way to pass it into the command
        /// </summary>
        /// <param name="baseRequest">The HttpClient request being sent</param>
        /// <param name="logger">An <see cref="ILogger"/></param>
        public UserDefinedHystrixCommand(Task<HttpResponseMessage> baseRequest, ILogger logger = null) : base(HystrixCommandGroupKeyDefault.AsKey("UserDefinedRandomFortuneCommand"))
        {
            _baseRequest = baseRequest;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> RunAsync()
        {
            // user-defined commands need to call the base request passed into the constructor
            return await _baseRequest;
        }

        protected override Task<HttpResponseMessage> RunFallbackAsync()
        {
            _logger?.LogInformation("Running {HystrixCommand} fallback", GetType().Name);
            return Task.FromResult(
                new HttpResponseMessage()
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(new Fortune() { Id = 9999, Text = "You will have a happy day!" }),
                        Encoding.UTF8,
                        "application/json"),
                    StatusCode = HttpStatusCode.ServiceUnavailable
                });
        }
    }
}

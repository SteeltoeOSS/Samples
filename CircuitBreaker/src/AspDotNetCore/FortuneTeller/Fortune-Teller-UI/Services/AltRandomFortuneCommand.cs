using Newtonsoft.Json;
using Steeltoe.CircuitBreaker.Hystrix;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fortune_Teller_UI.Services
{
    public class AltRandomFortuneCommand : HystrixCommand<HttpResponseMessage>
    {
        private readonly Task<HttpResponseMessage> _baseRequest;

        public AltRandomFortuneCommand(Task<HttpResponseMessage> baseRequest) : base(HystrixCommandGroupKeyDefault.AsKey("AltRandomFortuneCommand"))
        {
            _baseRequest = baseRequest;
        }

        protected override async Task<HttpResponseMessage> RunAsync()
        {
            return await _baseRequest;
        }

        protected override Task<HttpResponseMessage> RunFallbackAsync()
        {
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

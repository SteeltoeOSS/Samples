using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Steeltoe.Common.Http
{
    /// <summary>
    /// Wrap HttpClient calls with your own HystrixCommand
    /// </summary>
    /// <typeparam name="T">The type of the HystrixCommand that should be used</typeparam>
    public class HystrixHttpMessageHandler <T> : DelegatingHandler
        where T : HystrixCommand<HttpResponseMessage>
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;

        public HystrixHttpMessageHandler(ILoggerFactory loggerFactory = null)
        {
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory?.CreateLogger<HystrixHttpMessageHandler>();
        }

        /// <summary>
        /// Overly simplified imlementation!
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("Creating an instance of {HystrixCommandType}", typeof(T).GetTypeInfo().Name);

            // Create an instance of developer's HystrixCommand type, passing in the inner handler of this HttpClient request
            // This is ... less than ideal and probably worse than magic strings
            var command = Activator.CreateInstance(
                typeof(T), 
                BindingFlags.OptionalParamBinding, 
                new object[] { base.SendAsync(request, cancellationToken) });

            var result = await (command as HystrixCommand<HttpResponseMessage>).ExecuteAsync();

            return result;
        }
    }

    public class HystrixHttpMessageHandler : DelegatingHandler
    {
        private readonly Func<HttpResponseMessage> _fallback;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
        private readonly IHystrixCommandOptions _commandOptions;

        public HystrixHttpMessageHandler(IHystrixCommandOptions commandOptions = null, Func<HttpResponseMessage> fallback = null, ILoggerFactory loggerFactory = null)
        {
            _commandOptions = commandOptions;
            _fallback = fallback;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory?.CreateLogger<HystrixHttpMessageHandler>();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HystrixCommand<HttpResponseMessage> _hystrixCommand;
            if (_commandOptions != null)
            {
                _logger?.LogTrace("Creating HystrixCommand with user defined options under key {HystrixCommandGroupKey}", _commandOptions.CommandKey.Name);
                _hystrixCommand =
                    new HystrixCommand<HttpResponseMessage>(
                        _commandOptions,
                        SendCoreAsync(request, cancellationToken).GetAwaiter().GetResult,
                        _fallback,
                        _loggerFactory?.CreateLogger<HystrixCommand>());
            }
            else
            {
                _logger?.LogTrace("Creating HystrixCommand with key {HystrixCommandGroupKey}", request.RequestUri.Host);
                _hystrixCommand =
                    new HystrixCommand<HttpResponseMessage>(
                        HystrixCommandGroupKeyDefault.AsKey(request.RequestUri.Host),
                        () => SendCoreAsync(request, cancellationToken).Result,
                        _fallback,
                        _loggerFactory?.CreateLogger<HystrixCommand>());
            }

            var result = await _hystrixCommand.ExecuteAsync();

            return result;
        }

        protected virtual Task<HttpResponseMessage> SendCoreAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}

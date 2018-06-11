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
    /// Wrap <see cref="HttpClient"/> calls with a pre-defined <see cref="HystrixCommand{HttpResponseMessage}"/>
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="HystrixCommand"/> that should be used</typeparam>
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
        /// Overly simplified implementation!
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("Creating an instance of {HystrixCommandType}", typeof(T).GetTypeInfo().Name);

            // Create an instance of developer's HystrixCommand type, passing in the inner handler of this HttpClient request
            // This is ... less than ideal and probably worse than magic strings
            var command = (HystrixCommand<HttpResponseMessage>)Activator.CreateInstance(
                typeof(T), 
                //BindingFlags.OptionalParamBinding, 
                new object[] {
                    base.SendAsync(request, cancellationToken),
                    _loggerFactory?.CreateLogger<T>()
                });

            var result = await command.ExecuteAsync();

            return result;
        }
    }

    /// <summary>
    /// Wrap <see cref="HttpClient"/> calls with a generic <see cref="HystrixCommand{HttpResponseMessage}"/>
    /// </summary>
    public class HystrixHttpMessageHandler : DelegatingHandler
    {
        private readonly Func<HttpResponseMessage> _fallback;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
        private readonly IHystrixCommandOptions _commandOptions;

        /// <summary>
        /// Creates a <see cref="HystrixCommand"/> around outbound <see cref="HttpClient"/> requests 
        /// </summary>
        /// <param name="commandOptions">Configuration to use when creating the <see cref="HystrixCommand"/></param>
        /// <param name="fallback">Function to execute if the call fails</param>
        /// <param name="loggerFactory">For logging inside the handler and the command</param>
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

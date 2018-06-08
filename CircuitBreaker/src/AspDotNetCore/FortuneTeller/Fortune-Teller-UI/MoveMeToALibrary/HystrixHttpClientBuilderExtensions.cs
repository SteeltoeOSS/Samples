using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;
using System;
using System.Net.Http;

namespace Steeltoe.Common.Http
{
    public static class HystrixHttpClientBuilderExtensions
    {
        /// <summary>
        /// Add a Hystrix Circuit breaker to all outbound requests on this HttpClient
        /// </summary>
        /// <param name="builder">The <see cref="IHttpClientBuilder"/>.</param>
        /// <param name="commandOptions">Configuration for the hystrix command</param>
        /// <param name="fallback">Fallback method for all failed calls using this HttpClient</param>
        /// <param name="loggerFactory"></param>
        /// <returns>An <see cref="IHttpClientBuilder"/> that can be used to configure the client.</returns>
        public static IHttpClientBuilder AddCircuitBreaker(this IHttpClientBuilder builder, IHystrixCommandOptions commandOptions = null, Func<HttpResponseMessage> fallback = null, ILoggerFactory loggerFactory = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddHttpMessageHandler(() => new HystrixHttpMessageHandler(commandOptions, fallback, loggerFactory));
            return builder;
        }

        public static IHttpClientBuilder AddHystrixCommand<CommandType>(this IHttpClientBuilder builder)
            where CommandType: HystrixCommand<HttpResponseMessage>
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddHttpMessageHandler(() => new HystrixHttpMessageHandler<CommandType>());
            return builder;
        }
    }
}
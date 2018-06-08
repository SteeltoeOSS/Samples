using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;
using System;
using System.Net.Http;

namespace Steeltoe.Common.Http
{
    public static class HystrixHttpClientBuilderExtensions
    {
        // TO DECIDE: keep this, add more constructor options for on the fly command creation ... ?
        /// <summary>
        /// Add a Hystrix Circuit breaker to all outbound requests on this HttpClient
        /// </summary>
        /// <param name="builder">The <see cref="IHttpClientBuilder"/>.</param>
        /// <param name="commandOptions">Configuration for the hystrix command</param>
        /// <param name="fallback">Fallback method for all failed calls using this HttpClient</param>
        /// <param name="loggerFactory">To enable logging within the handler and the circuit breaker, provide an <see cref="ILoggerFactory"/></param>
        /// <returns>An <see cref="IHttpClientBuilder"/> that can be used to configure the client.</returns>
        public static IHttpClientBuilder AddHystrixCommand(this IHttpClientBuilder builder, IHystrixCommandOptions commandOptions = null, Func<HttpResponseMessage> fallback = null, ILoggerFactory loggerFactory = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddHttpMessageHandler(() => new HystrixHttpMessageHandler(commandOptions, fallback, loggerFactory));
            return builder;
        }

        /// <summary>
        /// Add a pre-defined HystrixCommand to all outbound requests on this HttpClient
        /// </summary>
        /// <typeparam name="CommandType">Your own <see cref="HystrixCommand{HttpResponseMessage}"/></typeparam>
        /// <param name="builder">The <see cref="IHttpClientBuilder"/>.</param>
        /// <param name="loggerFactory">To enable logging within the handler and the circuit breaker, provide an <see cref="ILoggerFactory"/></param>
        /// <returns>An <see cref="IHttpClientBuilder"/> that can be used to configure the client.</returns>
        public static IHttpClientBuilder AddHystrixCommand<T>(this IHttpClientBuilder builder, ILoggerFactory loggerFactory = null)
            where T: HystrixCommand<HttpResponseMessage>
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddHttpMessageHandler(() => new HystrixHttpMessageHandler<T>(loggerFactory));
            return builder;
        }
    }
}
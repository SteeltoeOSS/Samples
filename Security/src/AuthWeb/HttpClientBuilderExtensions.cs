using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;

namespace Steeltoe.Samples.AuthWeb;

/// <summary>
/// Provides simplified logging of outgoing HTTP requests.
/// </summary>
/// <remarks>
/// Based on https://josef.codes/customize-the-httpclient-logging-dotnet-core/.
/// </remarks>
public static class HttpClientBuilderExtensions
{
    public static IHttpClientBuilder ConfigureLogging(this IHttpClientBuilder builder)
    {
        builder.Services.TryAddScoped<HttpLogger>();
        return builder.RemoveAllLoggers().AddLogger<HttpLogger>(true);
    }

    private sealed class HttpLogger(ILogger<HttpLogger> logger) : IHttpClientLogger
    {
        private readonly ILogger<HttpLogger> _logger = logger;

        public object? LogRequestStart(HttpRequestMessage request)
        {
            _logger.LogInformation("Sending '{Request.Method}' to '{Request.Host}{Request.Path}'", request.Method,
                request.RequestUri?.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped), request.RequestUri?.PathAndQuery);

            return null;
        }

        public void LogRequestStop(object? context, HttpRequestMessage request, HttpResponseMessage response, TimeSpan elapsed)
        {
            _logger.LogInformation("Received '{Response.StatusCodeInt} {Response.StatusCodeString}' after {Response.ElapsedMilliseconds}ms",
                (int)response.StatusCode, response.StatusCode, elapsed.TotalMilliseconds.ToString("F1"));
        }

        public void LogRequestFailed(object? context, HttpRequestMessage request, HttpResponseMessage? response, Exception exception, TimeSpan elapsed)
        {
            _logger.LogError(exception, "Request towards '{Request.Host}{Request.Path}' failed after {Response.ElapsedMilliseconds}ms",
                request.RequestUri?.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped), request.RequestUri!.PathAndQuery,
                elapsed.TotalMilliseconds.ToString("F1"));
        }
    }
}

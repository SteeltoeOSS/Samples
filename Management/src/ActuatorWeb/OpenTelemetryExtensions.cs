using System.Text.RegularExpressions;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Instrumentation.Http;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Steeltoe.Common;
using B3Propagator = OpenTelemetry.Extensions.Propagators.B3Propagator;

namespace Steeltoe.Samples.ActuatorWeb;

internal static class OpenTelemetryExtensions
{
    private const string DefaultEgressIgnorePattern = "/api/v2/spans|/v2/apps/.*/permissions";

    public static void ConfigureOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenTelemetry().WithMetrics(metrics =>
        {
            metrics.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddRuntimeInstrumentation();
        }).WithTracing(tracing =>
        {
            tracing.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation();

            string? zipkinExporterAddress = configuration.GetValue<string>("OTEL_EXPORTER_ZIPKIN_ENDPOINT");

            if (!string.IsNullOrEmpty(zipkinExporterAddress))
            {
                tracing.AddZipkinExporter(zipkinExporterOptions => zipkinExporterOptions.Endpoint = new Uri(zipkinExporterAddress));
            }
        });

        services.ConfigureOpenTelemetryTracerProvider((serviceProvider, tracerProviderBuilder) =>
        {
            var appInfo = serviceProvider.GetRequiredService<IApplicationInstanceInfo>();
            tracerProviderBuilder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(appInfo.ApplicationName ?? "ActuatorWeb-fallback"));

            // For traces, use B3 (Zipkin) headers instead of W3C.
            List<TextMapPropagator> propagators =
            [
                new B3Propagator(),
                new BaggagePropagator()
            ];

            Sdk.SetDefaultTextMapPropagator(new CompositeTextMapPropagator(propagators));
        });

        // Avoid clogging tracing/metric stores with requests for Actuator Endpoints.
        services.AddOptions<AspNetCoreTraceInstrumentationOptions>().Configure(instrumentationOptions =>
        {
            instrumentationOptions.Filter += context => !context.Request.Path.StartsWithSegments("/actuator", StringComparison.OrdinalIgnoreCase) &&
                !context.Request.Path.StartsWithSegments("/cloudfoundryapplication", StringComparison.OrdinalIgnoreCase);
        });

        // Avoid clogging tracing/metric stores with Zipkin exports and Cloud Foundry permission checks (from Actuators).
        services.AddOptions<HttpClientTraceInstrumentationOptions>().Configure(instrumentationOptions =>
        {
            instrumentationOptions.FilterHttpRequestMessage += requestMessage =>
            {
                if (string.IsNullOrEmpty(requestMessage.RequestUri?.PathAndQuery))
                {
                    return false;
                }

                var pathMatcher = new Regex(DefaultEgressIgnorePattern, RegexOptions.None, TimeSpan.FromSeconds(1));

                return !pathMatcher.IsMatch(requestMessage.RequestUri.PathAndQuery);
            };
        });
    }
}

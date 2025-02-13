using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Steeltoe.Common;
using B3Propagator = OpenTelemetry.Extensions.Propagators.B3Propagator;

namespace Steeltoe.Samples.ActuatorApi;

internal static class OpenTelemetryExtensions
{
    public static void ConfigureOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenTelemetry().WithMetrics(meterProviderBuilder =>
        {
            meterProviderBuilder.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddRuntimeInstrumentation();
        }).WithTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder.AddAspNetCoreInstrumentation();

            string? zipkinExporterAddress = configuration.GetValue<string>("OTEL_EXPORTER_ZIPKIN_ENDPOINT");

            if (!string.IsNullOrEmpty(zipkinExporterAddress))
            {
                tracerProviderBuilder.AddZipkinExporter();
            }
        });

        services.ConfigureOpenTelemetryTracerProvider((serviceProvider, tracerProviderBuilder) =>
        {
            var appInfo = serviceProvider.GetRequiredService<IApplicationInstanceInfo>();
            tracerProviderBuilder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(appInfo.ApplicationName!));

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
    }
}

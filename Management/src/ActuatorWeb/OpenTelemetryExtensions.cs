using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Steeltoe.Common;

namespace Steeltoe.Samples.ActuatorWeb;

public static class OpenTelemetryExtensions
{
    public static void ConfigureOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        // Add OpenTelemetry
        services
            .AddOpenTelemetry()
            .WithTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder.AddAspNetCoreInstrumentation();

                var otlpExporterAddress = configuration.GetValue<string>("OpenTelemetry:OTLPExporterAddress");
                if (!string.IsNullOrEmpty(otlpExporterAddress))
                {
                    tracerProviderBuilder.AddOtlpExporter(otlpExporterOptions =>
                    {
                        otlpExporterOptions.Endpoint = new Uri(otlpExporterAddress);
                    });
                }
                var zipkinExporterAddress = configuration.GetValue<string>("OpenTelemetry:ZipkinExporterAddress");
                if (!string.IsNullOrEmpty(zipkinExporterAddress))
                {
                    tracerProviderBuilder.AddZipkinExporter(zipkinExporterOptions =>
                    {
                        zipkinExporterOptions.Endpoint = new Uri(zipkinExporterAddress);
                    });
                }
            });
        services.ConfigureOpenTelemetryTracerProvider((serviceProvider, tracerProviderBuilder) =>
        {
            var appInfo = serviceProvider.GetRequiredService<IApplicationInstanceInfo>();
            tracerProviderBuilder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(appInfo.ApplicationName ?? "ActuatorWeb-fallback"));
        });
    }
}

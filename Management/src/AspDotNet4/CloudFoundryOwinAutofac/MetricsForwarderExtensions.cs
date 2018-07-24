using Autofac;
using Microsoft.Extensions.Configuration;
using Steeltoe.Management.Exporter.Metrics;
using Steeltoe.Management.Exporter.Metrics.CloudFoundryForwarder;
using System;


namespace CloudFoundryOwinAutofac
{
    public static class MetricsForwarderExtensions
    {
        public static void RegisterMetricsForwarderExporter(this ContainerBuilder container, IConfiguration config)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            container.RegisterInstance(new CloudFoundryForwarderOptions(config)).SingleInstance();
            container.RegisterType<CloudFoundryForwarderExporter>().As<IMetricsExporter>().SingleInstance();
        }

        public static void StartMetricsExporter(this IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            if (container.TryResolve<IMetricsExporter>(out IMetricsExporter exporter))
            {
                exporter.Start();
            }
        }
    }
}
using Steeltoe.Management.Endpoint;

namespace Steeltoe.Samples.ActuatorWeb.CustomActuators.LocalTime;

public static class EndpointServiceCollectionExtensions
{
    /// <summary>
    /// Adds the local time actuator to the service container and configures the ASP.NET middleware pipeline.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection" /> to add services to.
    /// </param>
    /// <returns>
    /// The incoming <paramref name="services" /> so that additional calls can be chained.
    /// </returns>
    public static IServiceCollection AddLocalTimeActuator(this IServiceCollection services)
    {
        return services.AddLocalTimeActuator(true);
    }

    /// <summary>
    /// Adds the local time actuator to the service container.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection" /> to add services to.
    /// </param>
    /// <param name="configureMiddleware">
    /// When <c>false</c>, skips configuration of the ASP.NET middleware pipeline. While this provides full control over the pipeline order, it requires
    /// manual addition of the appropriate middleware for actuators to work correctly.
    /// </param>
    /// <returns>
    /// The incoming <paramref name="services" /> so that additional calls can be chained.
    /// </returns>
    public static IServiceCollection AddLocalTimeActuator(this IServiceCollection services, bool configureMiddleware)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddCoreActuatorServices<LocalTimeEndpointOptions, ConfigureLocalTimeEndpointOptions, LocalTimeEndpointMiddleware, ILocalTimeEndpointHandler,
            LocalTimeEndpointHandler, object?, string>(configureMiddleware);

        return services;
    }
}

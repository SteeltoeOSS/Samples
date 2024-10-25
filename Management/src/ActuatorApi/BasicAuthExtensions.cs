using idunno.Authentication.Basic;
using System.Security.Claims;

namespace Steeltoe.Samples.ActuatorApi;

public static class BasicAuthExtensions
{
    public static void ConfigureActuatorAuth(this IServiceCollection serviceCollection)
    {
        var builder = serviceCollection.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme);
        builder.AddBasic(BasicAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.ForwardDefaultSelector = httpContext => httpContext.Request.Path.StartsWithSegments("/actuator") ? BasicAuthenticationDefaults.AuthenticationScheme : null;
            options.ForwardChallenge = BasicAuthenticationDefaults.AuthenticationScheme;

            options.Events = new BasicAuthenticationEvents
            {
                OnValidateCredentials = (validateCredentialsContext) =>
                {
                    // This sample hard-codes the username and password for simplicity. In a real-world scenario, they are typically fetched from an external system.
                    if (validateCredentialsContext.Username == "actuatorUser" && validateCredentialsContext.Password == "actuatorPassword")
                    {
                        validateCredentialsContext.Principal = new ClaimsPrincipal(new ClaimsIdentity([new Claim("scope", "actuator.read")]));
                        validateCredentialsContext.Success();
                    }

                    return Task.CompletedTask;
                }
            };
        });

        serviceCollection.AddAuthorizationBuilder()
            .AddPolicy("actuator.read", policy => policy.RequireClaim("scope", "actuator.read"));
    }
}

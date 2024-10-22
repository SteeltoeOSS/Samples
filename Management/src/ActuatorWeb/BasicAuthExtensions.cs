// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using idunno.Authentication.Basic;
using System.Security.Claims;

namespace Steeltoe.Samples.ActuatorWeb;

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
                OnValidateCredentials = (context) =>
                {
                    // HardCoded as an example, do not use any of this in Production!
                    if (context.Username == "actuatorUser" && context.Password == "actuatorPassword")
                    {
                        context.Principal = new ClaimsPrincipal(new ClaimsIdentity([new Claim("scope", "actuator.read")]));
                        context.Success();
                    }

                    return Task.CompletedTask;
                }
            };
        });

        serviceCollection.AddAuthorizationBuilder()
            .AddPolicy("actuator.read", policy => policy.RequireClaim("scope", "actuator.read"));
    }
}

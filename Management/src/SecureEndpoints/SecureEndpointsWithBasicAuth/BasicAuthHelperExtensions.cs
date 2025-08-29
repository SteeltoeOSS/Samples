using idunno.Authentication.Basic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SecureEndpointsWithBasicAuth;

public static class BasicAuthHelperExtensions
{
    public static AuthenticationBuilder AddBasicAuth(this AuthenticationBuilder builder, string path, Claim claim) =>
        builder.AddBasic(BasicAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.ForwardDefaultSelector = httpContext =>
                httpContext.Request.Path.StartsWithSegments(path)
                    ? BasicAuthenticationDefaults.AuthenticationScheme
                    : null;
            options.ForwardChallenge = BasicAuthenticationDefaults.AuthenticationScheme;

            options.Events = new BasicAuthenticationEvents
            {
                OnValidateCredentials = context =>
                {
                    // HardCoded as example, don't use in Prod (or BasicAuth for that matter)
                    if (context.Username == "actuatorUser" && context.Password == "actuatorPassword")
                    {
                        context.Principal = new ClaimsPrincipal(new ClaimsIdentity([claim]));
                        context.Success();
                    }

                    return Task.CompletedTask;
                }
            };
        });
}
using idunno.Authentication.Basic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SecureEndpoints
{
    public static class BasicAuthHelperExtensions
    {
        public static AuthenticationBuilder
            AddBasicAuth(this AuthenticationBuilder builder, string path, Claim claim) =>
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
                        if (context.Username == "actuatorUser" &&
                            context.Password ==
                            "actuatorPassword") // HardCoded as example, don't use in Prod (or BasicAuth for that matter)
                        {
                            context.Principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { claim }));
                            context.Success();
                        }

                        return Task.CompletedTask;
                    }
                };
            });
    }
}
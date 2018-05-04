using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace CloudFoundryJwtAuthentication
{
    public class CustomClaimsAuthorizeAttribute : AuthorizeAttribute
    {
        readonly List<string> _claimTypes = new List<string>();
        public CustomClaimsAuthorizeAttribute(string requiredClaimType, params string[] requiredClaimTypes)
        {
            _claimTypes.Add(requiredClaimType);
            if (requiredClaimTypes != null)
            {
                _claimTypes.AddRange(requiredClaimTypes);
            }
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(actionContext.RequestContext.Principal.Identity);

            if (claimsPrincipal == null || !claimsPrincipal.Identity.IsAuthenticated)
            {
                Console.WriteLine("Received unauthorized request");
                return false;
            }

            return _claimTypes.Any(type => claimsPrincipal.HasClaim(claim => claim.Type == "scope" && claim.Value.Equals(type, StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}
using System.Linq;
using System.Security.Claims;

namespace CloudFoundrySingleSignon
{
    public class AuthorizationManager : ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            var requiredClaims = context.Action.Select(v => v.Value);
            var possessedClaims = context.Principal.Claims.Select(v => v.Value);

            var claim = ((ClaimsIdentity)context.Principal.Identity).FindFirst("testgroup");

            foreach (var c in requiredClaims)
            {
                if (possessedClaims.Contains(c))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
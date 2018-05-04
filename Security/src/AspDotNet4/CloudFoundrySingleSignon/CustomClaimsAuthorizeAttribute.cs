using System.Web.Mvc;
using System.Web.Routing;
using Thinktecture.IdentityModel.Authorization.Mvc;

namespace CloudFoundrySingleSignon
{
    public class CustomClaimsAuthorizeAttribute : ClaimsAuthorizeAttribute
    {
        public CustomClaimsAuthorizeAttribute(string action, params string[] resources) : base(action, resources)
        {
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "AccessDenied" }));
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}
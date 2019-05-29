using CloudFoundrySingleSignon.Models;
using Steeltoe.Security.Authentication.CloudFoundry;
using System.Web;
using System.Web.Mvc;

namespace CloudFoundrySingleSignon.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult AuthorizeSSO(string returnUrl)
        {
            // the value for provider must match the value used for AuthenticationType when configuring authentication (in Startup.Auth.cs)
            return new ChallengeResult(CloudFoundryDefaults.AuthenticationScheme, returnUrl ?? Url.Action("Secure", "Home"));
        }

        public ActionResult AccessDenied()
        {
            ViewData["Message"] = "Insufficient permissions.";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
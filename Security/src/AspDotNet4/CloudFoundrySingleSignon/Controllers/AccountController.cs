using CloudFoundrySingleSignon.Models;
using System.Web;
using System.Web.Mvc;

namespace CloudFoundrySingleSignon.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult AuthorizeSSO(string returnUrl)
        {
            return new ChallengeResult("PivotalSSO", returnUrl ?? Url.Action("Secure", "Home"));
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
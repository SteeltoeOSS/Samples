using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.DataProtection;

using Microsoft.AspNetCore.Http;

namespace RedisDataProtectionKeyStore.Controllers
{
    public class HomeController : Controller
    {
        IDataProtectionProvider _protection;
        private IHttpContextAccessor _httpContext;

        public HomeController(IDataProtectionProvider protection, IHttpContextAccessor contextAccessor)
        {
            _protection = protection;
            _httpContext = contextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public async Task<IActionResult> Protected()
        {
            ViewData["Message"] = "Protected page.";
            var session = _httpContext.HttpContext.Session;

        
            string protectedString = session.GetString("SomethingProtected");
            if (string.IsNullOrEmpty(protectedString)) {
                protectedString = "My Protected String - " + Guid.NewGuid().ToString();
                session.SetString("SomethingProtected",
                    _protection.CreateProtector("MyProtectedData in Session").Protect(protectedString));
                await session.CommitAsync();
            } else
            {
                protectedString = _protection.CreateProtector("MyProtectedData in Session").Unprotect(protectedString);
            }

            ViewData["SessionID"] = session.Id;
            ViewData["SomethingProtected"] = protectedString;
            ViewData["InstanceIndex"] = GetInstanceIndex();
            return View();
        }

        private string GetInstanceIndex()
        {
            return Environment.GetEnvironmentVariable("CF_INSTANCE_INDEX");
        }
    }
}

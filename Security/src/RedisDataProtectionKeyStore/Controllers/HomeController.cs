using System.Diagnostics;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using RedisDataProtectionKeyStore.Models;

namespace RedisDataProtectionKeyStore.Controllers;

public class HomeController : Controller
{
    private const string ProtectionPurpose = "SteeltoeDataProtectionInSession";
    private const string SessionKey = "ExampleSessionKey";

    private readonly IDataProtectionProvider _dataProtectionProvider;

    public HomeController(IDataProtectionProvider dataProtectionProvider)
    {
        _dataProtectionProvider = dataProtectionProvider;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        // Steeltoe: Obtain session information.
        IDataProtector dataProtector = _dataProtectionProvider.CreateProtector(ProtectionPurpose);
        string? sessionValue = HttpContext.Session.GetString(SessionKey);

        if (string.IsNullOrEmpty(sessionValue))
        {
            sessionValue = "Example Protected String - " + Guid.NewGuid();
            HttpContext.Session.SetString(SessionKey, dataProtector.Protect(sessionValue));
            await HttpContext.Session.CommitAsync(cancellationToken);
        }
        else
        {
            sessionValue = dataProtector.Unprotect(sessionValue);
        }

        var model = new SessionStateViewModel
        {
            InstanceIndex = Environment.GetEnvironmentVariable("CF_INSTANCE_INDEX"),
            SessionId = HttpContext.Session.Id,
            SessionValue = sessionValue
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}

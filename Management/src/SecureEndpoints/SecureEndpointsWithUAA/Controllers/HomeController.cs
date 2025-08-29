using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SecureEndpointsWithUAA.Controllers;

public class HomeController(ILogger<HomeController> logger) : Controller
{
    private readonly ILogger _logger = logger;
    private readonly Random _random = new();

    public IActionResult Index()
    {
        Thread.Sleep(_random.Next(500, 2000));

        _logger.LogCritical("Test Critical message");
        _logger.LogError("Test Error message");
        _logger.LogWarning("Test Warning message");
        _logger.LogInformation("Test Informational message");
        _logger.LogDebug("Test Debug message");
        _logger.LogTrace("Test Trace message");

        return View();
    }

    [Authorize(Policy = "fortunes.read")]
    public IActionResult About()
    {
        ViewData["Message"] = "Your About page.";
        return View("PageWithMessage");
    }

    #region Account-related

    [HttpPost]
    public async Task<IActionResult> LogOff()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction(nameof(Index), "Home");
    }

    [HttpGet]
    [Authorize]
    public IActionResult Login()
    {
        return RedirectToAction(nameof(Index), "Home");
    }

    public IActionResult Manage()
    {
        ViewData["Message"] = "Manage accounts using UAA or CF command line.";
        return View("PageWithMessage");
    }

    public IActionResult AccessDenied()
    {
        ViewData["Message"] = "Insufficient permissions.";
        return View();
    }

    #endregion

    public IActionResult Error()
    {
        throw new ArgumentException();
        //return View();
    }
}
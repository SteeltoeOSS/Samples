using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OAuth.Models;
using Steeltoe.Connectors.OAuth;

namespace OAuth.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly OAuthServiceOptions _options;

    public HomeController(ILogger<HomeController> logger, IOptions<OAuthServiceOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public IActionResult Index()
    {
        _options.Scope = new List<string>
        {
            "a",
            "b",
            "c",
            "d"
        };

        return View(new OAuthViewModel
        {
            Options = _options
        });
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

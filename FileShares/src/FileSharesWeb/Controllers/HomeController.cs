using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Steeltoe.Samples.FileSharesWeb.Models;

namespace Steeltoe.Samples.FileSharesWeb.Controllers;

public sealed class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
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

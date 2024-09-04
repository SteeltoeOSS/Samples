using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Steeltoe.Samples.FortuneTellerWeb.Models;
using Steeltoe.Samples.FortuneTellerWeb.Services;

namespace Steeltoe.Samples.FortuneTellerWeb.Controllers;

public sealed class HomeController(FortuneService fortuneService) : Controller
{
    private readonly FortuneService _fortuneService = fortuneService;

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        FortuneModel? model = await _fortuneService.GetRandomFortuneAsync(cancellationToken);
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

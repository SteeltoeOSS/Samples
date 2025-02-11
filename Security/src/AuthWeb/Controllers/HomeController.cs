using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steeltoe.Samples.AuthWeb.ApiClients;
using Steeltoe.Samples.AuthWeb.Models;

namespace Steeltoe.Samples.AuthWeb.Controllers;

public sealed class HomeController(
    JwtAuthorizationApiClient jwtAuthorizationApiClient, CertificateAuthorizationApiClient certificateAuthorizationApiClient) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [Authorize(Policy = Globals.RequiredJwtScope)]
    public IActionResult TestGroup()
    {
        ViewData["Message"] = $"You have the '{Globals.RequiredJwtScope}' permission.";
        return View();
    }

    [Authorize(Policy = Globals.UnknownJwtScope)]
    public IActionResult AnotherTestGroup()
    {
        ViewData["Message"] = $"You have the '{Globals.UnknownJwtScope}' permission.";

        return View("TestGroup");
    }

    [HttpGet]
    [Authorize]
    public IActionResult Login()
    {
        return RedirectToAction(nameof(Index), "Home");
    }

    public IActionResult Manage()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction(nameof(Index), "Home");
    }

    [Authorize]
    public async Task<IActionResult> InvokeJwtSample(CancellationToken cancellationToken)
    {
        string? accessToken = await HttpContext.GetTokenAsync("access_token");
        AuthApiResponseModel model = await jwtAuthorizationApiClient.GetAuthorizationAsync(accessToken, cancellationToken);
        return View("InvokeService", model);
    }

    public async Task<IActionResult> InvokeSameOrgSample(CancellationToken cancellationToken)
    {
        AuthApiResponseModel model = await certificateAuthorizationApiClient.GetSameOrgAsync(cancellationToken);
        return View("InvokeService", model);
    }

    public async Task<IActionResult> InvokeSameSpaceSample(CancellationToken cancellationToken)
    {
        AuthApiResponseModel model = await certificateAuthorizationApiClient.GetSameSpaceAsync(cancellationToken);
        return View("InvokeService", model);
    }

    public IActionResult AccessDenied()
    {
        ViewData["Message"] = "Insufficient permissions.";
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

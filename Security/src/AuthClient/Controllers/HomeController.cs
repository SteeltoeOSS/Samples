using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Steeltoe.Samples.AuthClient.Models;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Steeltoe.Samples.AuthClient.Controllers;

public sealed class HomeController(IHttpClientFactory clientFactory, ILogger<HomeController> logger) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    #region SSO

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
    public async Task<IActionResult> LogOff()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction(nameof(Index), "Home");
    }

    #endregion

    [Authorize(Policy = Globals.RequiredJwtScope)]
    public async Task<IActionResult> InvokeJwtSample()
    {
        using HttpClient jwtHttpClient = clientFactory.CreateClient("default");
        string? token = await HttpContext.GetTokenAsync("access_token");
        if (!string.IsNullOrEmpty(token))
        {
            jwtHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return View("InvokeService", await SendRequestToBackend(jwtHttpClient, "/api/JwtAuthorization"));
        }
        else
        {
            return View("InvokeService", model: "No access token found in user session. Perhaps you need to Authentication:Schemes:OpenIdConnect:SaveTokens to 'true'?");
        }
    }

    #region Client Certificate (Mutual TLS)

    public async Task<IActionResult> InvokeSameOrgSample()
    {
        using HttpClient mutualTlsHttpClient = clientFactory.CreateClient("AppInstanceIdentity");
        return View("InvokeService", await SendRequestToBackend(mutualTlsHttpClient, "/api/certificate/SameOrg"));
    }

    public async Task<IActionResult> InvokeSameSpaceSample()
    {
        using HttpClient mutualTlsHttpClient = clientFactory.CreateClient("AppInstanceIdentity");
        return View("InvokeService", await SendRequestToBackend(mutualTlsHttpClient, "/api/certificate/SameSpace"));
    }

    #endregion

    public IActionResult AccessDenied()
    {
        ViewData["Message"] = "Insufficient permissions.";
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private async Task<string> SendRequestToBackend(HttpClient client, string requestUri)
    {
        string result;
        try
        {
            logger.LogTrace("Sending request to {requestUri}", requestUri);
            result = await client.GetStringAsync(requestUri);
        }
        catch (Exception exception)
        {
            result = $"Request failed: {exception.Message}, at: {requestUri}";
        }
        return result;
    }
}
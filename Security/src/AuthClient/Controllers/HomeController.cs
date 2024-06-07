using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Steeltoe.Samples.AuthClient.Models;
using Steeltoe.Security.Authorization.Certificate;

namespace Steeltoe.Samples.AuthClient.Controllers;

public sealed class HomeController(IHttpClientFactory clientFactory, ILogger<HomeController> logger) : Controller
{
    private string? _backendBaseAddress;
    private readonly HttpClient _jwtHttpClient = clientFactory.CreateClient("default");
    private readonly HttpClient _mutualTlsHttpClient = clientFactory.CreateClient(CertificateAuthorizationDefaults.HttpClientName);
    private readonly ILogger<HomeController> _logger = logger;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _backendBaseAddress = GetSamplesUrl(HttpContext);
        base.OnActionExecuting(context);
    }

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

        return View();
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
        var token = await HttpContext.GetTokenAsync("access_token");
        if (!string.IsNullOrEmpty(token))
        {
            _jwtHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return View("InvokeService", await SendRequestToBackend(_jwtHttpClient, $"{_backendBaseAddress}/api/JwtAuthorization"));
        }
        else
        {
            return View("InvokeService", model: "No access token found in user session. Perhaps you need to Authentication:Schemes:OpenIdConnect:SaveTokens to 'true'?");
        }
    }

    #region Client Certificate (Mutual TLS)

    public async Task<IActionResult> InvokeSameOrgSample()
    {
        return View("InvokeService", await SendRequestToBackend(_mutualTlsHttpClient, $"{_backendBaseAddress}/api/certificate/SameOrg"));
    }

    public async Task<IActionResult> InvokeSameSpaceSample()
    {
        return View("InvokeService", await SendRequestToBackend(_mutualTlsHttpClient, $"{_backendBaseAddress}/api/certificate/SameSpace"));
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

    private const string FrontendHostname = "steeltoe-samples-authclient";
    private const string BackendHostname = "steeltoe-samples-authserver";
    private static string GetSamplesUrl(HttpContext httpContext)
    {
        string serviceHostname = httpContext.Request.Host.Host.Contains(FrontendHostname)
            ? httpContext.Request.Host.Host.Replace(FrontendHostname, BackendHostname)
            : "localhost:7184";

        return "https://" + serviceHostname;
    }

    private async Task<string> SendRequestToBackend(HttpClient client, string requestUri)
    {
        string result;
        try
        {
            _logger.LogTrace("Sending request to {requestUri}", requestUri);
            result = await client.GetStringAsync(requestUri);
        }
        catch (Exception e)
        {
            result = $"Request failed: {e.Message}, at: {requestUri}";
        }
        return result;
    }
}
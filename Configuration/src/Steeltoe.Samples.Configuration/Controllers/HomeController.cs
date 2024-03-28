using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Configuration.ConfigServer;
using Steeltoe.Samples.Configuration.Models;

namespace Steeltoe.Samples.Configuration.Controllers;

public sealed class HomeController(
    IOptionsSnapshot<ExternalConfiguration> configServerDataSnapshot, IOptionsSnapshot<ConfigServerClientSettingsOptions> configServerSettings,
    IOptionsSnapshot<PlaceholderValues> placeholderValues, IOptions<CloudFoundryApplicationOptions> appOptions,
    IOptions<CloudFoundryServicesOptions> serviceOptions, IConfiguration configuration) : Controller
{
    private readonly ExternalConfiguration _dataSnapshot  = configServerDataSnapshot.Value;

    private readonly ConfigServerClientSettingsOptions _configServerClientSettings  = configServerSettings.Value;

    private readonly PlaceholderValues _placeholderResolverValues  = placeholderValues.Value;

    private readonly CloudFoundryApplicationOptions _applicationOptions = appOptions.Value;
    private readonly CloudFoundryServicesOptions _serviceOptions = serviceOptions.Value;

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ExternalConfigurationData()
    {
        return View(_dataSnapshot);
    }

    public IActionResult ConfigServerSettings()
    {
        return View(_configServerClientSettings);
    }

    public IActionResult RandomValues()
    {
        return View(configuration);
    }

    public IActionResult PlaceholderValues()
    {
        return View(_placeholderResolverValues);
    }

    public IActionResult CloudFoundry()
    {
        return View(new CloudFoundryViewModel(_applicationOptions, _serviceOptions));
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

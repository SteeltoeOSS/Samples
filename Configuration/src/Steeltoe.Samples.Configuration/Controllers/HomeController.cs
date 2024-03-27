using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Steeltoe.Samples.Configuration.Models;
using Steeltoe.Configuration.ConfigServer;
using System.Diagnostics;
using Steeltoe.Configuration.CloudFoundry;

namespace Steeltoe.Samples.Configuration.Controllers;

public class HomeController(
    IOptionsSnapshot<ExternalConfiguration> configServerDataSnapshot,
    IOptionsSnapshot<ConfigServerClientSettingsOptions> configServerSettings,
    IOptionsSnapshot<PlaceholderValues> placeholderValues,
    IOptions<CloudFoundryApplicationOptions> appOptions,
    IOptions<CloudFoundryServicesOptions> serviceOptions,
    IConfiguration configuration)
    : Controller
{
    private ExternalConfiguration DataSnapshot { get; set; } = configServerDataSnapshot.Value;

    private ConfigServerClientSettingsOptions ConfigServerClientSettings { get; set; } = configServerSettings.Value;

    private PlaceholderValues PlaceholderResolverValues { get; set; } = placeholderValues.Value;

    private CloudFoundryApplicationOptions ApplicationOptions { get; } = appOptions.Value;
    private CloudFoundryServicesOptions ServiceOptions { get; } = serviceOptions.Value;

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ExternalConfigurationData()
    {
        return View(DataSnapshot);
    }

    public IActionResult ConfigServerSettings()
    {
        return View(ConfigServerClientSettings);
    }

    public IActionResult RandomValues()
    {
        return View(configuration);
    }

    public IActionResult PlaceholderValues()
    {
        return View(PlaceholderResolverValues);
    }

    public IActionResult CloudFoundry()
    {
        return View(new CloudFoundryViewModel(ApplicationOptions ?? new CloudFoundryApplicationOptions(), ServiceOptions ?? new CloudFoundryServicesOptions()));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

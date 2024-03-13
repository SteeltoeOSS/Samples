using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Steeltoe.Samples.Configuration.Models;
using Steeltoe.Configuration.ConfigServer;
using System.Diagnostics;
using Steeltoe.Configuration.CloudFoundry;

namespace Steeltoe.Samples.Configuration.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private ExternalConfiguration DataSnapshot { get; set; }

    private ConfigServerClientSettingsOptions ConfigServerClientSettings { get; set; }

    private PlaceholderValues PlaceholderResolverValues { get; set; }

    private CloudFoundryApplicationOptions ApplicationOptions { get; }
    private CloudFoundryServicesOptions ServiceOptions { get; }

    private IConfiguration _configuration;

    public HomeController(
        IOptionsSnapshot<ExternalConfiguration> configServerDataSnapshot,
        IOptionsSnapshot<ConfigServerClientSettingsOptions> configServerSettings,
        IOptionsSnapshot<PlaceholderValues> placeholderValues,
        IOptions<CloudFoundryApplicationOptions> appOptions,
        IOptions<CloudFoundryServicesOptions> serviceOptions,
        IConfiguration configuration,
        ILogger<HomeController> logger)
    {
        DataSnapshot = configServerDataSnapshot.Value;
        ConfigServerClientSettings = configServerSettings.Value;
        PlaceholderResolverValues = placeholderValues.Value;
        ApplicationOptions = appOptions.Value;
        ServiceOptions = serviceOptions.Value;
        _configuration = configuration;
        _logger = logger;
    }

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

    public ActionResult RandomValues()
    {
        return View(_configuration);
    }

    public ActionResult PlaceholderValues()
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

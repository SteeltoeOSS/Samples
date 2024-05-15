using FortuneTellerService.Models;
using Microsoft.AspNetCore.Mvc;

namespace FortuneTellerService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConfigurationController(IConfiguration configuration, ILogger<ConfigurationController> logger) : ControllerBase
{
    private const string ConfigurationKey = "foo";

    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<ConfigurationController> _logger = logger;

    [HttpGet]
    public ConfigurationLookupResult Get()
    {
        string? value = _configuration.GetValue<string?>(ConfigurationKey);

        if (value == null)
        {
            _logger.LogInformation($"GET api/configuration: key '{ConfigurationKey}' not found.");
            return new ConfigurationLookupResult();
        }

        string? configServerUri = _configuration.GetValue<string?>("Spring:Cloud:Config:Uri");
        _logger.LogInformation($"GET api/configuration: Resolved key '{ConfigurationKey}' to '{value}' from Config Server at '{configServerUri}'.");

        return new ConfigurationLookupResult
        {
            Value = value,
            Source = configServerUri
        };
    }
}

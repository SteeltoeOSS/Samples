using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using Steeltoe.Samples.DataProtection.Redis.Models;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Steeltoe.Samples.DataProtection.Redis.Pages;

public class IndexModel(IDataProtectionProvider dataProtectionProvider, ILogger<IndexModel> logger,
    IOptions<DataProtectionOptions> dpOptions, IDistributedCache distributedCache
    /*,
    IConnectionMultiplexer redis*/) : PageModel
{
    private const string ProtectionPurpose = "SteeltoeDataProtectionInSession";
    private const string SessionKey = "ExampleSessionKey";

    public SessionStateViewModel ProtectedData { get; set; } = default!;

    public async Task<IActionResult> OnGet(CancellationToken cancellationToken)
    {
        logger.LogInformation("Session id: {sessionID}, discriminator: {discriminator}",
            HttpContext.Session.Id,
            dpOptions.Value.ApplicationDiscriminator/*,
            , config: {configuration}redis.Configuration*/);

        // Steeltoe: Obtain session information.
        var dataProtector = dataProtectionProvider.CreateProtector(ProtectionPurpose);

        var sessionValue = HttpContext.Session.GetString(SessionKey);

        if (string.IsNullOrEmpty(sessionValue))
        {
            logger.LogInformation("No pre-existing session data was found for session");
            sessionValue = "Example Protected String - " + Guid.NewGuid();
            HttpContext.Session.SetString(SessionKey, dataProtector.Protect(sessionValue));
            await HttpContext.Session.CommitAsync(cancellationToken);
        }
        else
        {
            logger.LogInformation("  protected: {protectedValue}", sessionValue);
            sessionValue = dataProtector.Unprotect(sessionValue);
            logger.LogInformation("unprotected: {unprotectedValue}", sessionValue);
        }

        ProtectedData = new SessionStateViewModel
        {
            InstanceIndex = Environment.GetEnvironmentVariable("CF_INSTANCE_INDEX"),
            SessionId = HttpContext.Session.Id,
            SessionValue = sessionValue
        };

        return Page();
    }
}

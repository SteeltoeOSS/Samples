using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Steeltoe.Samples.RedisDataProtection.Models;

// Added test comment to trigger cibuild change.

namespace Steeltoe.Samples.RedisDataProtection.Pages;

public class IndexModel(
    IDataProtectionProvider dataProtectionProvider, ILogger<IndexModel> logger, IOptions<DataProtectionOptions> dataProtectionOptions) : PageModel
{
    private const string ProtectionPurpose = "SteeltoeDataProtectionInSession";
    private const string SessionKey = "ExampleSessionKey";

    public SessionStateViewModel? SessionState { get; set; }

    // Steeltoe: Obtain session information.
    public IActionResult OnGet(CancellationToken cancellationToken)
    {
        logger.LogInformation("Session ID: {SessionID}, discriminator: {Discriminator}", HttpContext.Session.Id,
            dataProtectionOptions.Value.ApplicationDiscriminator);

        IDataProtector dataProtector = dataProtectionProvider.CreateProtector(ProtectionPurpose);
        string? sessionValue = HttpContext.Session.GetString(SessionKey);
        string plainTextSessionValue;

        if (string.IsNullOrEmpty(sessionValue))
        {
            logger.LogInformation("No pre-existing session data was found, generating new.");

            plainTextSessionValue = $"Example Protected Text - {Guid.NewGuid()}";
            string cipherSessionValue = dataProtector.Protect(plainTextSessionValue);

            HttpContext.Session.SetString(SessionKey, cipherSessionValue);
        }
        else
        {
            plainTextSessionValue = dataProtector.Unprotect(sessionValue);

            logger.LogInformation("Found pre-existing session data. Stored value: '{StoredValue}', unprotected value: '{UnprotectedValue}'.", sessionValue,
                plainTextSessionValue);
        }

        SessionState = new SessionStateViewModel
        {
            InstanceIndex = Environment.GetEnvironmentVariable("CF_INSTANCE_INDEX"),
            SessionId = HttpContext.Session.Id,
            SessionValue = plainTextSessionValue
        };

        return Page();
    }
}

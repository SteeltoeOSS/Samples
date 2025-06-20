using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Steeltoe.Connectors;
using Steeltoe.Connectors.Redis;
using Steeltoe.Samples.RedisDataProtection.Models;

namespace Steeltoe.Samples.RedisDataProtection.Pages;

public class IndexModel(
    IDataProtectionProvider dataProtectionProvider, ILogger<IndexModel> logger, IOptions<DataProtectionOptions> dataProtectionOptions,
    ConnectorFactory<RedisOptions, IConnectionMultiplexer> connectorFactory) : PageModel
{
    private const string ProtectionPurpose = "SteeltoeDataProtectionInSession";
    private const string SessionKey = "ExampleSessionKey";
    private static readonly RedisKey DataProtectionKeysKey = "DataProtection-Keys";

    public List<string> DataProtectionXmlCryptoKeys { get; set; } = [];
    public SessionStateViewModel? SessionState { get; set; }

    public async Task<IActionResult> OnGet(CancellationToken cancellationToken)
    {
        logger.LogInformation("Session ID: {SessionID}, discriminator: {Discriminator}", HttpContext.Session.Id,
            dataProtectionOptions.Value.ApplicationDiscriminator);

        await GetRedisDataProtectionXmlCryptoKeysAsync();
        GetSessionState();

        return Page();
    }

    // Steeltoe: Retrieve XML that contains crypto keys from Redis.
    private async Task GetRedisDataProtectionXmlCryptoKeysAsync()
    {
        Connector<RedisOptions, IConnectionMultiplexer> connector = connectorFactory.Get();
        IConnectionMultiplexer connection = connector.GetConnection();
        IDatabase database = connection.GetDatabase();
        long keyCount = await database.ListLengthAsync(DataProtectionKeysKey);

        for (int keyIndex = 0; keyIndex < keyCount; keyIndex++)
        {
            string? elementValue = await database.ListGetByIndexAsync(DataProtectionKeysKey, keyIndex);

            if (elementValue != null)
            {
                string elementXmlValue = XDocument.Parse(elementValue).ToString();
                DataProtectionXmlCryptoKeys.Add(elementXmlValue);
            }
        }
    }

    // Steeltoe: Obtain session information.
    private void GetSessionState()
    {
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
    }
}

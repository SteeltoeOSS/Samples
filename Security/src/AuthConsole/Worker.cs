using Steeltoe.Samples.AuthConsole.ApiClients;
using Steeltoe.Samples.AuthConsole.Models;

namespace Steeltoe.Samples.AuthConsole;

public sealed class Worker(CertificateAuthorizationApiClient certificateAuthorizationApiClient, ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AuthApiResponseModel model = await certificateAuthorizationApiClient.GetSameOrgAsync(cancellationToken);
            logger.LogInformation("Request Uri: {requestUri}", model.RequestUri);
            logger.LogInformation("GetSameOrg response: {ApiResponse}", model.Message != null ? model.Message : model.Error);

            model = await certificateAuthorizationApiClient.GetSameSpaceAsync(cancellationToken);
            logger.LogInformation("Request Uri: {requestUri}", model.RequestUri);
            logger.LogInformation("GetSameOrg response: {ApiResponse}", model.Message != null ? model.Message : model.Error);

            Console.WriteLine("Sleeping for 10 seconds (press Ctrl+C to close).");
            await Task.Delay(10_000, cancellationToken);
        }
    }
}

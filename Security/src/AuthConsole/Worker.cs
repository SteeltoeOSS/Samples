using Steeltoe.Samples.AuthConsole.ApiClients;
using Steeltoe.Samples.AuthConsole.Models;

namespace Steeltoe.Samples.AuthConsole;

public sealed class Worker(CertificateAuthorizationApiClient certificateAuthorizationApiClient, ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine($"Background service starting at: {DateTimeOffset.Now} (press Ctrl+C to close).");

            AuthApiResponseModel model = await certificateAuthorizationApiClient.GetSameOrgAsync(cancellationToken);
            logger.LogInformation("GetSameOrg response: {ApiResponse}", model.Message != null ? model.Message : model.Error);

            model = await certificateAuthorizationApiClient.GetSameSpaceAsync(cancellationToken);
            logger.LogInformation("GetSameOrg response: {ApiResponse}", model.Message != null ? model.Message : model.Error);

            await Task.Delay(10_000, cancellationToken);
        }
    }
}

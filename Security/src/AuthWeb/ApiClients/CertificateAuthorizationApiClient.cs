using Steeltoe.Samples.AuthWeb.Models;

namespace Steeltoe.Samples.AuthWeb.ApiClients;

public sealed class CertificateAuthorizationApiClient(HttpClient httpClient)
    : StringApiClient(httpClient)
{
    public async Task<AuthApiResponseModel> GetSameOrgAsync(CancellationToken cancellationToken)
    {
        return await GetAsync("api/certificate/SameOrg", cancellationToken);
    }

    public async Task<AuthApiResponseModel> GetSameSpaceAsync(CancellationToken cancellationToken)
    {
        return await GetAsync("api/certificate/SameSpace", cancellationToken);
    }
}

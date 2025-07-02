using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Steeltoe.Samples.AuthWeb.Models;

namespace Steeltoe.Samples.AuthWeb.ApiClients;

public sealed class JwtAuthorizationApiClient(HttpClient httpClient)
    : StringApiClient(httpClient)
{
    public async Task<AuthApiResponseModel> GetAuthorizationAsync(string? accessToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            return new AuthApiResponseModel
            {
                Error = new InvalidOperationException(
                    "No access token found in user session. Perhaps you need to set 'Authentication:Schemes:OpenIdConnect:SaveTokens' to 'true'?")
            };
        }

        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return await GetAsync("api/JwtAuthorization", cancellationToken);
    }
}

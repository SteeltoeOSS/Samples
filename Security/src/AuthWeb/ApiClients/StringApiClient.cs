using Steeltoe.Samples.AuthWeb.Models;

namespace Steeltoe.Samples.AuthWeb.ApiClients;

public abstract class StringApiClient(HttpClient httpClient)
{
    protected HttpClient HttpClient => httpClient;

    protected async Task<AuthApiResponseModel> GetAsync(string requestUri, CancellationToken cancellationToken)
    {
        string fullRequestUri = httpClient.BaseAddress + requestUri;

        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync(requestUri, cancellationToken);
            string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return new AuthApiResponseModel
                {
                    RequestUri = fullRequestUri,
                    Message = responseBody
                };
            }

            throw new HttpRequestException($"Request failed with status {(int)response.StatusCode}:{Environment.NewLine}{responseBody}");
        }
        catch (Exception exception)
        {
            return new AuthApiResponseModel
            {
                RequestUri = fullRequestUri,
                Error = exception
            };
        }
    }
}

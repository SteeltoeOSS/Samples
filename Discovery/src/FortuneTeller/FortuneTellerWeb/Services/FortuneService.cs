using Steeltoe.Samples.FortuneTellerWeb.Models;

namespace Steeltoe.Samples.FortuneTellerWeb.Services;

public sealed class FortuneService(HttpClient httpClient, ILogger<FortuneService> logger)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<FortuneService> _logger = logger;

    public async Task<FortuneModel?> GetRandomFortuneAsync(CancellationToken cancellationToken)
    {
        var fortune = await _httpClient.GetFromJsonAsync<FortuneModel>("random", cancellationToken);
        _logger.LogInformation("Returning fortune {FortuneModel}.", fortune);

        return fortune;
    }
}

﻿using Steeltoe.Common.HealthChecks;

namespace FortuneTellerService;

internal sealed class ExampleHealthContributor(HealthStatus? healthStatus) : IHealthContributor
{
    private readonly HealthStatus? _healthStatus = healthStatus;

    public string Id => "Example";

    public Task<HealthCheckResult?> CheckHealthAsync(CancellationToken cancellationToken)
    {
        HealthCheckResult? result = _healthStatus != null
            ? new HealthCheckResult
            {
                Status = _healthStatus.Value
            }
            : null;

        return Task.FromResult(result);
    }
}
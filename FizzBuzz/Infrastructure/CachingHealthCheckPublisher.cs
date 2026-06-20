using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FizzBuzz.Infrastructure;

public class CachingHealthCheckPublisher : IHealthCheckPublisher
{
    private readonly HealthReportCache _cache;

    public CachingHealthCheckPublisher(HealthReportCache cache)
    {
        _cache = cache;
    }

    public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
    {
        _cache.Report = report;
        return Task.CompletedTask;
    }
}

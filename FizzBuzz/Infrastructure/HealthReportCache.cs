using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FizzBuzz.Infrastructure;

public class HealthReportCache
{
    private volatile HealthReport? _report;

    public HealthReport? Report
    {
        get => _report;
        set => _report = value;
    }
}

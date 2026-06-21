using FizzBuzz.Infrastructure;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FizzBuzz.Presentation.HealthChecks;

public static class HealthCheckEndpoints
{
    public static IEndpointRouteBuilder MapHealthCheckEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/health/live", (HealthReportCache cache) => Build(cache, _ => false))
            .WithName("HealthLive")
            .WithTags("healthcheck");

        app.MapGet("/health/ready", (HealthReportCache cache) => Build(cache, entry => entry.Tags.Contains("ready")))
            .WithName("HealthReady")
            .WithTags("healthcheck");

        return app;
    }

    private static IResult Build(HealthReportCache cache, Func<HealthReportEntry, bool> filter)
    {
        var report = cache.Report;
        if (report is null)
        {
            return Results.Text(HealthStatus.Unhealthy.ToString(), statusCode: StatusCodes.Status503ServiceUnavailable);
        }

        var entries = report.Entries.Values.Where(filter).ToList();

        var status = entries.Count == 0
            ? HealthStatus.Healthy
            : entries.Min(entry => entry.Status);

        var statusCode = status == HealthStatus.Unhealthy
            ? StatusCodes.Status503ServiceUnavailable
            : StatusCodes.Status200OK;

        return Results.Text(status.ToString(), statusCode: statusCode);
    }
}

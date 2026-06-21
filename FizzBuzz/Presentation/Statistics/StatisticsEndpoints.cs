using FizzBuzz.Application;
using FizzBuzz.Security;

namespace FizzBuzz.Presentation.Statistics;

public static class StatisticsEndpoints
{
    public static IEndpointRouteBuilder MapStatisticsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/statistics", HandleStatistics)
            .WithName("GetStatistics")
            .RequireAuthorization(AuthorizationPolicies.StatisticsAdmin);

        return app;
    }

    public static async Task<IResult> HandleStatistics(
        IFizzBuzzStatisticsRepository statisticsRepository,
        ILogger<StatisticsEndpointsLogger> logger,
        CancellationToken cancellationToken = default)
    {
        logger.LogDebug("Statistics request received");

        var statistics = await statisticsRepository.GetMostFrequentAsync(cancellationToken);

        var response = statistics
            .Select(statistic => new StatisticsResponse(
                new StatisticsRequestResponse(
                    statistic.Request.Int1,
                    statistic.Request.Int2,
                    statistic.Request.Limit,
                    statistic.Request.Str1,
                    statistic.Request.Str2),
                statistic.Hits))
            .ToList();

        logger.LogInformation("Returning {Count} most frequent FizzBuzz request(s)", response.Count);

        return Results.Ok(response);
    }
}

public sealed class StatisticsEndpointsLogger;

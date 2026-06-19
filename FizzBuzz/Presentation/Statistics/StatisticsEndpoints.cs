using FizzBuzz.Application;

namespace FizzBuzz.Presentation.Statistics;

public static class StatisticsEndpoints
{
    public static IEndpointRouteBuilder MapStatisticsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/statistics", HandleStatistics).WithName("GetStatistics");

        return app;
    }

    public static IResult HandleStatistics(IFizzBuzzStatisticsRepository statisticsRepository)
    {
        var statistics = statisticsRepository.GetMostFrequent();

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

        return Results.Ok(response);
    }
}

using FizzBuzz.Domain;
using FizzBuzz.Infrastructure;
using FizzBuzz.Presentation;
using FizzBuzz.Presentation.Statistics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;

namespace FizzBuzz.Test;

public class StatisticsEndpointsTests
{
    private readonly InMemoryFizzBuzzStatisticsRepository _statisticsRepository = new();
    private readonly ILogger<StatisticsEndpointsLogger> _logger = NullLogger<StatisticsEndpointsLogger>.Instance;

    [Fact]
    public async Task Should_return_ok_with_empty_list_when_no_request_recorded()
    {
        var result = await StatisticsEndpoints.HandleStatistics(_statisticsRepository, _logger);

        var ok = result.ShouldBeOfType<Ok<List<StatisticsResponse>>>();
        ok.StatusCode.ShouldBe(StatusCodes.Status200OK);
        ok.Value.ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_return_ok_with_most_frequent_request_and_hits()
    {
        var request = FizzBuzzRequest.Create(int1: 3, int2: 5, limit: 10, str1: "fizz", str2: "buzz").Value!;
        await _statisticsRepository.AddAsync(request);
        await _statisticsRepository.AddAsync(request);

        var result = await StatisticsEndpoints.HandleStatistics(_statisticsRepository, _logger);

        var ok = result.ShouldBeOfType<Ok<List<StatisticsResponse>>>();
        ok.StatusCode.ShouldBe(StatusCodes.Status200OK);
        ok.Value.ShouldHaveSingleItem()
            .ShouldBe(new StatisticsResponse(
                new StatisticsRequestResponse(Int1: 3, Int2: 5, Limit: 10, Str1: "fizz", Str2: "buzz"),
                Hits: 2));
    }

    [Fact]
    public async Task Should_return_all_tied_requests()
    {
        var first = FizzBuzzRequest.Create(int1: 3, int2: 5, limit: 10, str1: "fizz", str2: "buzz").Value!;
        var second = FizzBuzzRequest.Create(int1: 2, int2: 7, limit: 20, str1: "foo", str2: "bar").Value!;
        await _statisticsRepository.AddAsync(first);
        await _statisticsRepository.AddAsync(second);

        var result = await StatisticsEndpoints.HandleStatistics(_statisticsRepository, _logger);

        var ok = result.ShouldBeOfType<Ok<List<StatisticsResponse>>>();
        ok.Value!.Count.ShouldBe(2);
    }
}

using FizzBuzz.Domain;
using FizzBuzz.Infrastructure;
using FizzBuzz.Presentation;
using FizzBuzz.Presentation.Statistics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Shouldly;

namespace FizzBuzz.Test;

public class StatisticsEndpointsTests
{
    private readonly InMemoryFizzBuzzStatisticsRepository _statisticsRepository = new();

    [Fact]
    public void Should_return_ok_with_empty_list_when_no_request_recorded()
    {
        var result = StatisticsEndpoints.HandleStatistics(_statisticsRepository);

        var ok = result.ShouldBeOfType<Ok<List<StatisticsResponse>>>();
        ok.StatusCode.ShouldBe(StatusCodes.Status200OK);
        ok.Value.ShouldBeEmpty();
    }

    [Fact]
    public void Should_return_ok_with_most_frequent_request_and_hits()
    {
        var request = new FizzBuzzRequest(Int1: 3, Int2: 5, Limit: 10, Str1: "fizz", Str2: "buzz");
        _statisticsRepository.Add(request);
        _statisticsRepository.Add(request);

        var result = StatisticsEndpoints.HandleStatistics(_statisticsRepository);

        var ok = result.ShouldBeOfType<Ok<List<StatisticsResponse>>>();
        ok.StatusCode.ShouldBe(StatusCodes.Status200OK);
        ok.Value.ShouldHaveSingleItem()
            .ShouldBe(new StatisticsResponse(
                new StatisticsRequestResponse(Int1: 3, Int2: 5, Limit: 10, Str1: "fizz", Str2: "buzz"),
                Hits: 2));
    }

    [Fact]
    public void Should_return_all_tied_requests()
    {
        var first = new FizzBuzzRequest(Int1: 3, Int2: 5, Limit: 10, Str1: "fizz", Str2: "buzz");
        var second = new FizzBuzzRequest(Int1: 2, Int2: 7, Limit: 20, Str1: "foo", Str2: "bar");
        _statisticsRepository.Add(first);
        _statisticsRepository.Add(second);

        var result = StatisticsEndpoints.HandleStatistics(_statisticsRepository);

        var ok = result.ShouldBeOfType<Ok<List<StatisticsResponse>>>();
        ok.Value!.Count.ShouldBe(2);
    }
}

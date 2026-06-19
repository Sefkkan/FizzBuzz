using FizzBuzz.Domain;
using FizzBuzz.Infrastructure;
using Shouldly;

namespace FizzBuzz.Test;

public class InMemoryFizzBuzzStatisticsRepositoryTests
{
    private readonly InMemoryFizzBuzzStatisticsRepository _statisticsRepository = new();

    [Fact]
    public void Should_return_empty_list_when_no_request_recorded()
    {
        _statisticsRepository.GetMostFrequent().ShouldBeEmpty();
    }

    [Fact]
    public void Should_count_hits_for_repeated_identical_requests()
    {
        var request = new FizzBuzzRequest(Int1: 3, Int2: 5, Limit: 10, Str1: "fizz", Str2: "buzz");

        _statisticsRepository.Add(request);
        _statisticsRepository.Add(request);
        _statisticsRepository.Add(request);

        var result = _statisticsRepository.GetMostFrequent();

        var statistic = result.ShouldHaveSingleItem();
        statistic.Request.ShouldBe(request);
        statistic.Hits.ShouldBe(3);
    }

    [Fact]
    public void Should_return_only_the_most_frequent_request()
    {
        var frequent = new FizzBuzzRequest(Int1: 3, Int2: 5, Limit: 10, Str1: "fizz", Str2: "buzz");
        var rare = new FizzBuzzRequest(Int1: 2, Int2: 7, Limit: 20, Str1: "foo", Str2: "bar");

        _statisticsRepository.Add(frequent);
        _statisticsRepository.Add(frequent);
        _statisticsRepository.Add(rare);

        var result = _statisticsRepository.GetMostFrequent();

        var statistic = result.ShouldHaveSingleItem();
        statistic.Request.ShouldBe(frequent);
        statistic.Hits.ShouldBe(2);
    }

    [Fact]
    public void Should_return_all_requests_when_they_are_tied()
    {
        var first = new FizzBuzzRequest(Int1: 3, Int2: 5, Limit: 10, Str1: "fizz", Str2: "buzz");
        var second = new FizzBuzzRequest(Int1: 2, Int2: 7, Limit: 20, Str1: "foo", Str2: "bar");

        _statisticsRepository.Add(first);
        _statisticsRepository.Add(first);
        _statisticsRepository.Add(second);
        _statisticsRepository.Add(second);

        var result = _statisticsRepository.GetMostFrequent();

        result.Count.ShouldBe(2);
        result.ShouldAllBe(statistic => statistic.Hits == 2);
        result.Select(statistic => statistic.Request).ShouldBe(new[] { first, second }, ignoreOrder: true);
    }

    [Fact]
    public void Should_treat_requests_with_different_parameters_as_distinct()
    {
        var request = new FizzBuzzRequest(Int1: 3, Int2: 5, Limit: 10, Str1: "fizz", Str2: "buzz");
        var differentLimit = new FizzBuzzRequest(Int1: 3, Int2: 5, Limit: 20, Str1: "fizz", Str2: "buzz");

        _statisticsRepository.Add(request);
        _statisticsRepository.Add(differentLimit);

        var result = _statisticsRepository.GetMostFrequent();

        result.Count.ShouldBe(2);
    }
}

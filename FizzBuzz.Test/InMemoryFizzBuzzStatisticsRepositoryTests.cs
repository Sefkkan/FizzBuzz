using FizzBuzz.Domain;
using FizzBuzz.Infrastructure;
using Shouldly;

namespace FizzBuzz.Test;

public class InMemoryFizzBuzzStatisticsRepositoryTests
{
    private readonly InMemoryFizzBuzzStatisticsRepository _statisticsRepository = new();

    [Fact]
    public async Task Should_return_empty_list_when_no_request_recorded()
    {
        (await _statisticsRepository.GetMostFrequentAsync()).ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_count_hits_for_repeated_identical_requests()
    {
        var request = FizzBuzzRequest.Create(int1: 3, int2: 5, limit: 10, str1: "fizz", str2: "buzz").Value!;

        await _statisticsRepository.AddAsync(request);
        await _statisticsRepository.AddAsync(request);
        await _statisticsRepository.AddAsync(request);

        var result = await _statisticsRepository.GetMostFrequentAsync();

        var statistic = result.ShouldHaveSingleItem();
        statistic.Request.ShouldBe(request);
        statistic.Hits.ShouldBe(3);
    }

    [Fact]
    public async Task Should_return_only_the_most_frequent_request()
    {
        var frequent = FizzBuzzRequest.Create(int1: 3, int2: 5, limit: 10, str1: "fizz", str2: "buzz").Value!;
        var rare = FizzBuzzRequest.Create(int1: 2, int2: 7, limit: 20, str1: "foo", str2: "bar").Value!;

        await _statisticsRepository.AddAsync(frequent);
        await _statisticsRepository.AddAsync(frequent);
        await _statisticsRepository.AddAsync(rare);

        var result = await _statisticsRepository.GetMostFrequentAsync();

        var statistic = result.ShouldHaveSingleItem();
        statistic.Request.ShouldBe(frequent);
        statistic.Hits.ShouldBe(2);
    }

    [Fact]
    public async Task Should_return_all_requests_when_they_are_tied()
    {
        var first = FizzBuzzRequest.Create(int1: 3, int2: 5, limit: 10, str1: "fizz", str2: "buzz").Value!;
        var second = FizzBuzzRequest.Create(int1: 2, int2: 7, limit: 20, str1: "foo", str2: "bar").Value!;

        await _statisticsRepository.AddAsync(first);
        await _statisticsRepository.AddAsync(first);
        await _statisticsRepository.AddAsync(second);
        await _statisticsRepository.AddAsync(second);

        var result = await _statisticsRepository.GetMostFrequentAsync();

        result.Count.ShouldBe(2);
        result.ShouldAllBe(statistic => statistic.Hits == 2);
        result.Select(statistic => statistic.Request).ShouldBe(new[] { first, second }, ignoreOrder: true);
    }

    [Fact]
    public async Task Should_treat_requests_with_different_parameters_as_distinct()
    {
        var request = FizzBuzzRequest.Create(int1: 3, int2: 5, limit: 10, str1: "fizz", str2: "buzz").Value!;
        var differentLimit = FizzBuzzRequest.Create(int1: 3, int2: 5, limit: 20, str1: "fizz", str2: "buzz").Value!;

        await _statisticsRepository.AddAsync(request);
        await _statisticsRepository.AddAsync(differentLimit);

        var result = await _statisticsRepository.GetMostFrequentAsync();

        result.Count.ShouldBe(2);
    }
}

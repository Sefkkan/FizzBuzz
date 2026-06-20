using System.Text.Json;
using FizzBuzz.Application;
using FizzBuzz.Domain;
using StackExchange.Redis;

namespace FizzBuzz.Infrastructure;

public class RedisFizzBuzzStatisticsRepository : IFizzBuzzStatisticsRepository
{
    private const string SetKey = "fizzbuzz:stats";
    private readonly IConnectionMultiplexer _redis;

    public RedisFizzBuzzStatisticsRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task AddAsync(FizzBuzzRequest request, CancellationToken cancellationToken = default)
    {
        await _redis.GetDatabase().SortedSetIncrementAsync(SetKey, Serialize(request), 1);
    }

    public async Task<IReadOnlyList<FizzBuzzStatistic>> GetMostFrequentAsync(CancellationToken cancellationToken = default)
    {
        var db = _redis.GetDatabase();

        var top = await db.SortedSetRangeByRankWithScoresAsync(SetKey, start: -1, stop: -1);
        if (top.Length == 0)
        {
            return [];
        }

        var maxHits = top[0].Score;

        var winners = await db.SortedSetRangeByScoreAsync(SetKey, start: maxHits, stop: maxHits);

        return winners
            .Select(member => new FizzBuzzStatistic(Deserialize(member!), (int)maxHits))
            .ToList();
    }

    private static string Serialize(FizzBuzzRequest request) =>
        JsonSerializer.Serialize(new FizzBuzzRequestData(
            request.Int1, request.Int2, request.Limit, request.Str1, request.Str2));

    private static FizzBuzzRequest Deserialize(string field)
    {
        var data = JsonSerializer.Deserialize<FizzBuzzRequestData>(field)!;
        return FizzBuzzRequest.Create(data.Int1, data.Int2, data.Limit, data.Str1, data.Str2).Value!;
    }

    private record FizzBuzzRequestData(int Int1, int Int2, int Limit, string Str1, string Str2);
}

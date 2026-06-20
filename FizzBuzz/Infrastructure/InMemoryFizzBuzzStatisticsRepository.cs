using System.Collections.Concurrent;
using FizzBuzz.Application;
using FizzBuzz.Domain;

namespace FizzBuzz.Infrastructure;

public class InMemoryFizzBuzzStatisticsRepository : IFizzBuzzStatisticsRepository
{
    private readonly ConcurrentDictionary<FizzBuzzRequest, int> _hits = new();

    public Task AddAsync(FizzBuzzRequest request, CancellationToken cancellationToken = default)
    {
        _hits.AddOrUpdate(request, addValue: 1, updateValueFactory: (_, count) => count + 1);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<FizzBuzzStatistic>> GetMostFrequentAsync(CancellationToken cancellationToken = default)
    {
        var snapshot = _hits.ToArray();
        if (snapshot.Length == 0)
        {
            return Task.FromResult<IReadOnlyList<FizzBuzzStatistic>>([]);
        }

        var maxHits = snapshot.Max(pair => pair.Value);
        IReadOnlyList<FizzBuzzStatistic> result = snapshot
            .Where(pair => pair.Value == maxHits)
            .Select(pair => new FizzBuzzStatistic(pair.Key, pair.Value))
            .ToList();

        return Task.FromResult(result);
    }
}

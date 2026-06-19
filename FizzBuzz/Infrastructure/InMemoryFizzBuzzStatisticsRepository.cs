using System.Collections.Concurrent;
using FizzBuzz.Application;
using FizzBuzz.Domain;

namespace FizzBuzz.Infrastructure;

public class InMemoryFizzBuzzStatisticsRepository : IFizzBuzzStatisticsRepository
{
    private readonly ConcurrentDictionary<FizzBuzzRequest, int> _hits = new();

    public void Add(FizzBuzzRequest request)
    {
        _hits.AddOrUpdate(request, addValue: 1, updateValueFactory: (_, count) => count + 1);
    }

    public IReadOnlyList<FizzBuzzStatistic> GetMostFrequent()
    {
        var snapshot = _hits.ToArray();
        if (snapshot.Length == 0)
        {
            return [];
        }

        var maxHits = snapshot.Max(pair => pair.Value);
        return snapshot
            .Where(pair => pair.Value == maxHits)
            .Select(pair => new FizzBuzzStatistic(pair.Key, pair.Value))
            .ToList();
    }
}

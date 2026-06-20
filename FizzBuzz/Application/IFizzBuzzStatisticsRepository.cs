using FizzBuzz.Domain;

namespace FizzBuzz.Application;

public interface IFizzBuzzStatisticsRepository
{
    Task AddAsync(FizzBuzzRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FizzBuzzStatistic>> GetMostFrequentAsync(CancellationToken cancellationToken = default);
}

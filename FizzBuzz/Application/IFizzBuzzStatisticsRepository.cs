using FizzBuzz.Domain;

namespace FizzBuzz.Application;

public interface IFizzBuzzStatisticsRepository
{
    void Add(FizzBuzzRequest request);

    IReadOnlyList<FizzBuzzStatistic> GetMostFrequent();
}

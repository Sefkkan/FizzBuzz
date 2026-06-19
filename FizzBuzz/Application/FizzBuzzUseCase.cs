using FizzBuzz.Domain;

namespace FizzBuzz.Application;

public class FizzBuzzUseCase : IFizzBuzzUseCase
{
    private readonly FizzBuzzService _service;
    private readonly IFizzBuzzStatisticsRepository _statisticsRepository;

    public FizzBuzzUseCase(FizzBuzzService service, IFizzBuzzStatisticsRepository statisticsRepository)
    {
        _service = service;
        _statisticsRepository = statisticsRepository;
    }

    public List<string> Execute(FizzBuzzRequest request)
    {
        var result = _service.GenerateSequence(request);
        _statisticsRepository.Add(request);
        return result;
    }
}

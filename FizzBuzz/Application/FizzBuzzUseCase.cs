using FizzBuzz.Domain;
using Microsoft.Extensions.Logging;

namespace FizzBuzz.Application;

public class FizzBuzzUseCase : IFizzBuzzUseCase
{
    private readonly FizzBuzzService _fizzBuzzService;
    private readonly IFizzBuzzStatisticsRepository _statisticsRepository;
    private readonly ILogger<FizzBuzzUseCase> _logger;

    public FizzBuzzUseCase(
        FizzBuzzService fizzBuzzService,
        IFizzBuzzStatisticsRepository statisticsRepository,
        ILogger<FizzBuzzUseCase> logger)
    {
        _fizzBuzzService = fizzBuzzService;
        _statisticsRepository = statisticsRepository;
        _logger = logger;
    }

    public List<string> Execute(FizzBuzzRequest request)
    {
        _logger.LogDebug("Executing FizzBuzz use case for Limit={Limit}", request.Limit);

        var result = _fizzBuzzService.GenerateSequence(request);
        _statisticsRepository.Add(request);

        _logger.LogDebug("Statistics hit recorded for FizzBuzz request with Limit={Limit}", request.Limit);
        return result;
    }
}

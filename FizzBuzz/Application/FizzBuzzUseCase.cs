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

    public async Task<List<string>> ExecuteAsync(FizzBuzzRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Executing FizzBuzz use case for Limit={Limit}", request.Limit);

        var result = _fizzBuzzService.GenerateSequence(request);
        await _statisticsRepository.AddAsync(request, cancellationToken);

        _logger.LogDebug("Statistics hit recorded for FizzBuzz request with Limit={Limit}", request.Limit);
        return result;
    }
}

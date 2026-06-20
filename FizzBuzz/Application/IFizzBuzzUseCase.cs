using FizzBuzz.Domain;

namespace FizzBuzz.Application;

public interface IFizzBuzzUseCase
{
    Task<List<string>> ExecuteAsync(FizzBuzzRequest request, CancellationToken cancellationToken = default);
}

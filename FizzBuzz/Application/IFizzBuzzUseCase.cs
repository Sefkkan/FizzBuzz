using FizzBuzz.Domain;

namespace FizzBuzz.Application;

public interface IFizzBuzzUseCase
{
    List<string> Execute(FizzBuzzRequest request);
}

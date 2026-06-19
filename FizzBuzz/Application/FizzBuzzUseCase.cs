using FizzBuzz.Domain;

namespace FizzBuzz.Application;

public class FizzBuzzUseCase : IFizzBuzzUseCase
{
    private readonly FizzBuzzService _service;

    public FizzBuzzUseCase(FizzBuzzService service)
    {
        _service = service;
    }

    public List<string> Execute(FizzBuzzRequest request)
    {
        return _service.GenerateSequence(request);
    }
}

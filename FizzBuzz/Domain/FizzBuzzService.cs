namespace FizzBuzz.Domain;

public class FizzBuzzService
{
    private readonly IFizzBuzzEvaluator _evaluator;

    public FizzBuzzService(IFizzBuzzEvaluator evaluator)
    {
        _evaluator = evaluator;
    }

    public List<string> GenerateSequence(FizzBuzzRequest request)
    {
        var result = new List<string>();

        for (var i = 1; i <= request.Limit; i++)
        {
            result.Add(_evaluator.Evaluate(i, request));
        }

        return result;
    }
}

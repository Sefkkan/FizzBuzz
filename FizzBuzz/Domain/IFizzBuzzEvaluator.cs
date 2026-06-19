namespace FizzBuzz.Domain;

public interface IFizzBuzzEvaluator
{
    string Evaluate(int number, FizzBuzzRequest request);
}

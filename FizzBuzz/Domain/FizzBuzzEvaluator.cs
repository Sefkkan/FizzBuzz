namespace FizzBuzz.Domain;

public class FizzBuzzEvaluator
{
    public string Evaluate(int number, FizzBuzzRequest request)
    {
        if (number % request.Int1 == 0 && number % request.Int2 == 0)
        {
            return request.Str1 + request.Str2;
        }

        if (number % request.Int1 == 0)
        {
            return request.Str1;
        }

        if (number % request.Int2 == 0)
        {
            return request.Str2;
        }

        return number.ToString();
    }
}

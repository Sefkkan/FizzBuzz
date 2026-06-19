namespace FizzBuzz.Presentation.FizzBuzz;

public static class FizzBuzzRequestValidator
{
    private const int MaxLimit = 1_000_000;

    public static Dictionary<string, string[]> Validate(int int1, int int2, int limit, string? str1, string? str2)
    {
        var errors = new Dictionary<string, string[]>();

        if (int1 == 0)
        {
            errors[nameof(int1)] = ["int1 must not be zero."];
        }

        if (int2 == 0)
        {
            errors[nameof(int2)] = ["int2 must not be zero."];
        }

        if (limit < 0)
        {
            errors[nameof(limit)] = ["limit must not be negative."];
        }
        else if (limit > MaxLimit)
        {
            errors[nameof(limit)] = [$"limit must not exceed {MaxLimit}."];
        }

        if (string.IsNullOrEmpty(str1))
        {
            errors[nameof(str1)] = ["str1 must not be empty."];
        }

        if (string.IsNullOrEmpty(str2))
        {
            errors[nameof(str2)] = ["str2 must not be empty."];
        }

        return errors;
    }
}

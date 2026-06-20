namespace FizzBuzz.Domain;

public record FizzBuzzRequest
{
    public const int MaxLimit = 50_000;

    private FizzBuzzRequest(int int1, int int2, int limit, string str1, string str2)
    {
        Int1 = int1;
        Int2 = int2;
        Limit = limit;
        Str1 = str1;
        Str2 = str2;
    }

    public int Int1 { get; }

    public int Int2 { get; }

    public int Limit { get; }

    public string Str1 { get; }

    public string Str2 { get; }
    
    public static Result<FizzBuzzRequest> Create(int int1, int int2, int limit, string str1, string str2)
    {
        var errors = new Dictionary<string, string[]>();

        if (int1 == 0)
        {
            errors["int1"] = ["int1 must not be zero."];
        }

        if (int2 == 0)
        {
            errors["int2"] = ["int2 must not be zero."];
        }

        if (limit < 0)
        {
            errors["limit"] = ["limit must not be negative."];
        }
        else if (limit > MaxLimit)
        {
            errors["limit"] = [$"limit must not exceed {MaxLimit}."];
        }

        if (string.IsNullOrEmpty(str1))
        {
            errors["str1"] = ["str1 must not be empty."];
        }

        if (string.IsNullOrEmpty(str2))
        {
            errors["str2"] = ["str2 must not be empty."];
        }

        return errors.Count > 0
            ? Result<FizzBuzzRequest>.Failure(errors)
            : Result<FizzBuzzRequest>.Success(new FizzBuzzRequest(int1, int2, limit, str1, str2));
    }
}

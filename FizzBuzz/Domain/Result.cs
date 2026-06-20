namespace FizzBuzz.Domain;

public class Result<T>
{
    private static readonly IReadOnlyDictionary<string, string[]> NoErrors = new Dictionary<string, string[]>();

    private Result(bool isSuccess, T? value, IReadOnlyDictionary<string, string[]> errors)
    {
        IsSuccess = isSuccess;
        Value = value;
        Errors = errors;
    }

    public bool IsSuccess { get; }

    public T? Value { get; }

    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public static Result<T> Success(T value) => new(true, value, NoErrors);

    public static Result<T> Failure(IReadOnlyDictionary<string, string[]> errors) => new(false, default, errors);
}

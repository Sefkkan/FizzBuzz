namespace FizzBuzz.Domain;

public record FizzBuzzRequest(int Int1, int Int2, int Limit, string Str1, string Str2)
{
    public int Int1 { get; } = Int1 != 0
        ? Int1
        : throw new ArgumentException("int1 must not be zero.", nameof(Int1));

    public int Int2 { get; } = Int2 != 0
        ? Int2
        : throw new ArgumentException("int2 must not be zero.", nameof(Int2));

    public int Limit { get; } = Limit >= 0
        ? Limit
        : throw new ArgumentException("limit must not be negative.", nameof(Limit));

    public string Str1 { get; } = !string.IsNullOrEmpty(Str1)
        ? Str1
        : throw new ArgumentException("str1 must not be empty.", nameof(Str1));

    public string Str2 { get; } = !string.IsNullOrEmpty(Str2)
        ? Str2
        : throw new ArgumentException("str2 must not be empty.", nameof(Str2));
}

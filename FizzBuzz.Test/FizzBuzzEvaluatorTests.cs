using FizzBuzz.Domain;
using Shouldly;

namespace FizzBuzz.Test;

public class FizzBuzzEvaluatorTests
{
    private readonly FizzBuzzEvaluator _evaluator = new();
    private readonly FizzBuzzRequest _defaultRequest = new(Int1: 3, Int2: 5, Limit: 100, Str1: "fizz", Str2: "buzz");

    [Fact]
    public void Should_return_str1_when_divisible_by_int1_only()
    {
        var result = _evaluator.Evaluate(3, _defaultRequest);

        result.ShouldBe("fizz");
    }

    [Fact]
    public void Should_return_str2_when_divisible_by_int2_only()
    {
        var result = _evaluator.Evaluate(5, _defaultRequest);

        result.ShouldBe("buzz");
    }

    [Fact]
    public void Should_return_str1str2_when_divisible_by_both()
    {
        var result = _evaluator.Evaluate(15, _defaultRequest);

        result.ShouldBe("fizzbuzz");
    }

    [Fact]
    public void Should_return_number_when_not_divisible_by_either()
    {
        var result = _evaluator.Evaluate(1, _defaultRequest);

        result.ShouldBe("1");
    }
}

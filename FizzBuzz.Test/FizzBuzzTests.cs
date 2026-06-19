using FizzBuzz.Domain;
using Shouldly;

namespace FizzBuzz.Test;

public class FizzBuzzTests
{
    private readonly FizzBuzzService _service = new(new FizzBuzzEvaluator());

    [Fact]
    public void Should_return_correct_sequence_for_classic_fizzbuzz()
    {
        var request = new FizzBuzzRequest(Int1: 3, Int2: 5, Limit: 15, Str1: "fizz", Str2: "buzz");

        var result = _service.GenerateSequence(request);

        result.ShouldBe(new[] { "1", "2", "fizz", "4", "buzz", "fizz", "7", "8", "fizz", "buzz", "11", "fizz", "13", "14", "fizzbuzz" });
    }

    [Fact]
    public void Should_return_empty_list_when_limit_is_zero()
    {
        var request = new FizzBuzzRequest(Int1: 3, Int2: 5, Limit: 0, Str1: "fizz", Str2: "buzz");

        var result = _service.GenerateSequence(request);

        result.ShouldBeEmpty();
    }

    [Theory]
    [InlineData(3,  "fizz")]
    [InlineData(5,  "buzz")]
    [InlineData(15, "fizzbuzz")]
    [InlineData(1,  "1")]
    public void Should_return_expected_result_for_number(int number, string expected)
    {
        var request = new FizzBuzzRequest(Int1: 3, Int2: 5, Limit: number, Str1: "fizz", Str2: "buzz");

        var result = _service.GenerateSequence(request);

        result.Last().ShouldBe(expected);
    }
}

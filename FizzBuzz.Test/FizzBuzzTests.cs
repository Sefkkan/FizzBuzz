using FizzBuzz.Domain;
using Shouldly;

namespace FizzBuzz.Test;

public class FizzBuzzTests
{
    private readonly FizzBuzzService _service = new(new FizzBuzzEvaluator());

    [Fact]
    public void Should_return_correct_sequence_for_classic_fizzbuzz()
    {
        var request = FizzBuzzRequest.Create(int1: 3, int2: 5, limit: 15, str1: "fizz", str2: "buzz").Value!;

        var result = _service.GenerateSequence(request);

        result.ShouldBe(new[] { "1", "2", "fizz", "4", "buzz", "fizz", "7", "8", "fizz", "buzz", "11", "fizz", "13", "14", "fizzbuzz" });
    }

    [Fact]
    public void Should_return_empty_list_when_limit_is_zero()
    {
        var request = FizzBuzzRequest.Create(int1: 3, int2: 5, limit: 0, str1: "fizz", str2: "buzz").Value!;

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
        var request = FizzBuzzRequest.Create(int1: 3, int2: 5, limit: number, str1: "fizz", str2: "buzz").Value!;

        var result = _service.GenerateSequence(request);

        result.Last().ShouldBe(expected);
    }
}

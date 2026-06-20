using FizzBuzz.Domain;
using Shouldly;

namespace FizzBuzz.Test;

public class FizzBuzzRequestTests
{
    [Theory]
    [InlineData(0, 5, 10, "fizz", "buzz", "int1")]
    [InlineData(3, 0, 10, "fizz", "buzz", "int2")]
    [InlineData(3, 5, -1, "fizz", "buzz", "limit")]
    [InlineData(3, 5, 10, "", "buzz", "str1")]
    [InlineData(3, 5, 10, "fizz", "", "str2")]
    public void Should_fail_when_a_parameter_is_invalid(int int1, int int2, int limit, string str1, string str2, string expectedErrorKey)
    {
        var result = FizzBuzzRequest.Create(int1, int2, limit, str1, str2);

        result.IsSuccess.ShouldBeFalse();
        result.Errors.Keys.ShouldContain(expectedErrorKey);
    }

    [Fact]
    public void Should_fail_when_limit_exceeds_the_maximum()
    {
        var result = FizzBuzzRequest.Create(int1: 3, int2: 5, limit: FizzBuzzRequest.MaxLimit + 1, str1: "fizz", str2: "buzz");

        result.IsSuccess.ShouldBeFalse();
        result.Errors.Keys.ShouldContain("limit");
    }

    [Fact]
    public void Should_succeed_when_all_parameters_are_valid()
    {
        var result = FizzBuzzRequest.Create(int1: 3, int2: 5, limit: 10, str1: "fizz", str2: "buzz");

        result.IsSuccess.ShouldBeTrue();
        var request = result.Value.ShouldNotBeNull();
        request.Int1.ShouldBe(3);
        request.Int2.ShouldBe(5);
        request.Limit.ShouldBe(10);
        request.Str1.ShouldBe("fizz");
        request.Str2.ShouldBe("buzz");
    }
}

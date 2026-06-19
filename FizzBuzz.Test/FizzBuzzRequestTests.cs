using FizzBuzz.Domain;
using Shouldly;

namespace FizzBuzz.Test;

public class FizzBuzzRequestTests
{
    [Fact]
    public void Should_throw_when_int1_is_zero()
    {
        Should.Throw<ArgumentException>(() => new FizzBuzzRequest(Int1: 0, Int2: 5, Limit: 10, Str1: "fizz", Str2: "buzz"));
    }

    [Fact]
    public void Should_throw_when_int2_is_zero()
    {
        Should.Throw<ArgumentException>(() => new FizzBuzzRequest(Int1: 3, Int2: 0, Limit: 10, Str1: "fizz", Str2: "buzz"));
    }

    [Fact]
    public void Should_throw_when_limit_is_negative()
    {
        Should.Throw<ArgumentException>(() => new FizzBuzzRequest(Int1: 3, Int2: 5, Limit: -1, Str1: "fizz", Str2: "buzz"));
    }

    [Fact]
    public void Should_throw_when_str1_is_empty()
    {
        Should.Throw<ArgumentException>(() => new FizzBuzzRequest(Int1: 3, Int2: 5, Limit: 10, Str1: "", Str2: "buzz"));
    }

    [Fact]
    public void Should_throw_when_str2_is_empty()
    {
        Should.Throw<ArgumentException>(() => new FizzBuzzRequest(Int1: 3, Int2: 5, Limit: 10, Str1: "fizz", Str2: ""));
    }

    [Fact]
    public void Should_create_request_when_all_parameters_are_valid()
    {
        var request = new FizzBuzzRequest(Int1: 3, Int2: 5, Limit: 10, Str1: "fizz", Str2: "buzz");

        request.Int1.ShouldBe(3);
        request.Int2.ShouldBe(5);
        request.Limit.ShouldBe(10);
        request.Str1.ShouldBe("fizz");
        request.Str2.ShouldBe("buzz");
    }
}

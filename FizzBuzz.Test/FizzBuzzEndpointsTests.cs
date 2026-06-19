using FizzBuzz.Application;
using FizzBuzz.Domain;
using FizzBuzz.Infrastructure;
using FizzBuzz.Presentation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Shouldly;

namespace FizzBuzz.Test;

public class FizzBuzzEndpointsTests
{
    private readonly IFizzBuzzUseCase _useCase = new FizzBuzzUseCase(new FizzBuzzService(new FizzBuzzEvaluator()), new InMemoryFizzBuzzStatisticsRepository());

    [Fact]
    public void Should_return_ok_with_sequence_when_request_is_valid()
    {
        var result = FizzBuzzEndpoints.HandleFizzBuzz(int1: 3, int2: 5, limit: 5, str1: "fizz", str2: "buzz", _useCase);

        var ok = result.ShouldBeOfType<Ok<List<string>>>();
        ok.StatusCode.ShouldBe(StatusCodes.Status200OK);
        ok.Value.ShouldBe(new[] { "1", "2", "fizz", "4", "buzz" });
    }

    [Theory]
    [InlineData(0, 5, 10, "fizz", "buzz")]
    [InlineData(3, 0, 10, "fizz", "buzz")]
    [InlineData(3, 5, -1, "fizz", "buzz")]
    [InlineData(3, 5, 10, "", "buzz")]
    [InlineData(3, 5, 10, "fizz", "")]
    public void Should_return_bad_request_when_a_parameter_is_invalid(int int1, int int2, int limit, string str1, string str2)
    {
        var result = FizzBuzzEndpoints.HandleFizzBuzz(int1, int2, limit, str1, str2, _useCase);

        var problem = result.ShouldBeOfType<ProblemHttpResult>();
        problem.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
    }
}

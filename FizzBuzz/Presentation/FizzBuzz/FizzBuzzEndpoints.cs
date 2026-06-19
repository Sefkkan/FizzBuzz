using FizzBuzz.Application;
using FizzBuzz.Domain;

namespace FizzBuzz.Presentation;

public static class FizzBuzzEndpoints
{
    public static IEndpointRouteBuilder MapFizzBuzzEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/fizzbuzz", HandleFizzBuzz).WithName("GetFizzBuzz");

        return app;
    }

    public static IResult HandleFizzBuzz(
        int int1,
        int int2,
        int limit,
        string str1,
        string str2,
        IFizzBuzzUseCase useCase)
    {
        var errors = FizzBuzzRequestValidator.Validate(int1, int2, limit, str1, str2);
        if (errors.Count > 0)
        {
            return Results.ValidationProblem(errors);
        }

        var request = new FizzBuzzRequest(int1, int2, limit, str1, str2);
        var result = useCase.Execute(request);
        return Results.Ok(result);
    }
}

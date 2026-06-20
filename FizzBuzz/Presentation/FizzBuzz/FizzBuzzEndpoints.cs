using FizzBuzz.Application;
using FizzBuzz.Domain;

namespace FizzBuzz.Presentation.FizzBuzz;

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
        var result = FizzBuzzRequest.Create(int1, int2, limit, str1, str2);
        if (!result.IsSuccess)
        {
            return Results.ValidationProblem(result.Errors);
        }

        var sequence = useCase.Execute(result.Value!);
        return Results.Ok(sequence);
    }
}

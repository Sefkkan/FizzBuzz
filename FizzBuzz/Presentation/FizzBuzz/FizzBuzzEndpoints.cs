using FizzBuzz.Application;
using FizzBuzz.Domain;
using FizzBuzz.Presentation.Utils;

namespace FizzBuzz.Presentation.FizzBuzz;

public static class FizzBuzzEndpoints
{
    public static IEndpointRouteBuilder MapFizzBuzzEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/fizzbuzz", HandleFizzBuzz)
            .WithName("GetFizzBuzz")
            .RequireRateLimiting(RateLimiting.FizzBuzzPolicy);

        return app;
    }

    public static async Task<IResult> HandleFizzBuzz(
        int int1,
        int int2,
        int limit,
        string str1,
        string str2,
        IFizzBuzzUseCase useCase,
        ILogger<FizzBuzzEndpointsLogger> logger,
        CancellationToken cancellationToken = default)
    {
        logger.LogDebug(
            "FizzBuzz request received: Int1={Int1}, Int2={Int2}, Limit={Limit}, Str1={Str1}, Str2={Str2}",
            int1, int2, limit, str1, str2);

        var result = FizzBuzzRequest.Create(int1, int2, limit, str1, str2);
        if (!result.IsSuccess)
        {
            logger.LogWarning(
                "FizzBuzz request validation failed for invalid fields: {InvalidFields}",
                string.Join(", ", result.Errors.Keys));
            return Results.ValidationProblem(result.Errors);
        }

        var sequence = await useCase.ExecuteAsync(result.Value!, cancellationToken);
        logger.LogInformation(
            "FizzBuzz sequence generated with {Count} entries for Limit={Limit}",
            sequence.Count, limit);
        return Results.Ok(sequence);
    }
}

public sealed class FizzBuzzEndpointsLogger;

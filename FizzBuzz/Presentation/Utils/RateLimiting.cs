using System.Threading.RateLimiting;

namespace FizzBuzz.Presentation.Utils;

public static class RateLimiting
{
    /// <summary>
    /// Stricter policy applied to the FizzBuzz endpoint, which can generate
    /// large sequences and write to Redis on every call.
    /// </summary>
    public const string FizzBuzzPolicy = "fizzbuzz";

    public static IServiceCollection AddFizzBuzzRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: GetPartitionKey(context),
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100,
                        Window = TimeSpan.FromMinutes(1)
                    }));

            options.AddPolicy(FizzBuzzPolicy, context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: GetPartitionKey(context),
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 20,
                        Window = TimeSpan.FromMinutes(1)
                    }));
        });

        return services;
    }

    private static string GetPartitionKey(HttpContext context) =>
        context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
}

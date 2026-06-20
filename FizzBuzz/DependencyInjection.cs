using FizzBuzz.Application;
using FizzBuzz.Domain;
using FizzBuzz.Infrastructure;
using StackExchange.Redis;

namespace FizzBuzz;

public static class DependencyInjection
{
    public static IServiceCollection AddFizzBuzz(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFizzBuzzEvaluator, FizzBuzzEvaluator>();
        services.AddScoped<FizzBuzzService>();
        services.AddScoped<IFizzBuzzUseCase, FizzBuzzUseCase>();

        var redisConnectionString = configuration.GetConnectionString("Redis")
                                    ?? throw new InvalidOperationException("Missing 'Redis' connection string.");

        services.AddSingleton<IConnectionMultiplexer>(
            _ => ConnectionMultiplexer.Connect(redisConnectionString));

        services.AddSingleton<IFizzBuzzStatisticsRepository, RedisFizzBuzzStatisticsRepository>();

        return services;
    }
}

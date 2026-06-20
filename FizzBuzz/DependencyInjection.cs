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

        var redisOptions = ConfigurationOptions.Parse(redisConnectionString);
        redisOptions.AbortOnConnectFail = false;
        redisOptions.ConnectTimeout = 2000;
        redisOptions.ConnectRetry = 1;

        services.AddSingleton<IConnectionMultiplexer>(
            _ => ConnectionMultiplexer.Connect(redisOptions));

        services.AddSingleton<IFizzBuzzStatisticsRepository, RedisFizzBuzzStatisticsRepository>();

        services.AddHealthChecks()
            .AddCheck<RedisHealthCheck>("redis", tags: ["ready"]);

        return services;
    }
}

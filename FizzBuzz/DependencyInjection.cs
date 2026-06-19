using FizzBuzz.Application;
using FizzBuzz.Domain;
using FizzBuzz.Infrastructure;

namespace FizzBuzz;

public static class DependencyInjection
{
    public static IServiceCollection AddFizzBuzz(this IServiceCollection services)
    {
        services.AddScoped<IFizzBuzzEvaluator, FizzBuzzEvaluator>();
        services.AddScoped<FizzBuzzService>();
        services.AddScoped<IFizzBuzzUseCase, FizzBuzzUseCase>();

        services.AddSingleton<IFizzBuzzStatisticsRepository, InMemoryFizzBuzzStatisticsRepository>();

        return services;
    }
}

using FizzBuzz.Application;
using FizzBuzz.Domain;

namespace FizzBuzz;

public static class DependencyInjection
{
    public static IServiceCollection AddFizzBuzz(this IServiceCollection services)
    {
        services.AddScoped<IFizzBuzzEvaluator, FizzBuzzEvaluator>();
        services.AddScoped<FizzBuzzService>();
        services.AddScoped<IFizzBuzzUseCase, FizzBuzzUseCase>();

        return services;
    }
}

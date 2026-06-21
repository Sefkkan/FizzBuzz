using FizzBuzz.Application;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace FizzBuzz;

public static class Observability
{
    public const string DefaultOtlpEndpoint = "http://localhost:4317";

    public static string GetServiceName(this IConfiguration configuration) =>
        configuration["OpenTelemetry:ServiceName"] ?? "fizzbuzz";

    public static string GetOtlpEndpoint(this IConfiguration configuration) =>
        configuration["OpenTelemetry:OtlpEndpoint"] ?? DefaultOtlpEndpoint;

    public static IServiceCollection AddObservability(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceName = configuration.GetServiceName();
        var otlpEndpoint = new Uri(configuration.GetOtlpEndpoint());

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracing => tracing
                .AddSource(AppDiagnostics.ActivitySourceName)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRedisInstrumentation()
                .AddOtlpExporter(options => options.Endpoint = otlpEndpoint))
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddOtlpExporter(options => options.Endpoint = otlpEndpoint));

        return services;
    }
}

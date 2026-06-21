using FizzBuzz;
using FizzBuzz.Presentation.FizzBuzz;
using FizzBuzz.Presentation.HealthChecks;
using FizzBuzz.Presentation.Statistics;
using FizzBuzz.Presentation.Utils;
using Microsoft.OpenApi;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    var serviceName = builder.Configuration.GetServiceName();
    var otlpEndpoint = builder.Configuration.GetOtlpEndpoint();

    builder.Services.AddSerilog((services, configuration) => configuration
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.OpenTelemetry(options =>
        {
            options.Endpoint = otlpEndpoint;
            options.Protocol = OtlpProtocol.Grpc;
            options.ResourceAttributes = new Dictionary<string, object>
            {
                ["service.name"] = serviceName
            };
        }));

    builder.Services.AddObservability(builder.Configuration);

    builder.Services.AddOpenApi(options => options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0);
    builder.Services.AddProblemDetails();
    builder.Services.AddFizzBuzz(builder.Configuration);
    builder.Services.AddFizzBuzzRateLimiting();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "FizzBuzz API"));

    app.UseHttpsRedirection();

    // Health check probes must stay outside rate limiting.
    app.MapHealthCheckEndpoints();

    app.UseRateLimiter();

    var v1 = app.MapGroup("/api/v1").WithTags("v1");
    v1.MapFizzBuzzEndpoints();
    v1.MapStatisticsEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "FizzBuzz API terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

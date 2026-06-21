using FizzBuzz;
using FizzBuzz.Presentation.FizzBuzz;
using FizzBuzz.Presentation.HealthChecks;
using FizzBuzz.Presentation.Statistics;
using FizzBuzz.Presentation.Utils;
using FizzBuzz.Security;
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

    builder.Services.AddOpenApi(options =>
    {
        options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
        options.AddDocumentTransformer<OAuthSecuritySchemeTransformer>();
    });
    builder.Services.AddProblemDetails();
    builder.Services.AddFizzBuzz(builder.Configuration);
    builder.Services.AddFizzBuzzRateLimiting();
    builder.Services.AddKeycloakAuthentication(builder.Configuration);
    builder.Services.AddFizzBuzzAuthorization();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "FizzBuzz API");
        options.OAuthClientId(app.Configuration["Keycloak:SwaggerClientId"]);
        options.OAuthUsePkce();
        options.OAuthScopes("openid", "profile");
    });

    app.UseHttpsRedirection();

    // Health check probes must stay outside rate limiting and authentication.
    app.MapHealthCheckEndpoints();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseRateLimiter();

    var v1 = app.MapGroup("/api/v1").WithTags("fizzbuzz");
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

// Exposed so the integration test project can boot the app through WebApplicationFactory.
public partial class Program;

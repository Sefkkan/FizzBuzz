using FizzBuzz;
using FizzBuzz.Presentation;
using FizzBuzz.Presentation.FizzBuzz;
using FizzBuzz.Presentation.Statistics;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(options => options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0);
builder.Services.AddProblemDetails();
builder.Services.AddFizzBuzz();

var app = builder.Build();

app.MapOpenApi();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "FizzBuzz API"));

app.UseHttpsRedirection();

var v1 = app.MapGroup("/api/v1").WithTags("v1");
v1.MapFizzBuzzEndpoints();
v1.MapStatisticsEndpoints();

app.Run();

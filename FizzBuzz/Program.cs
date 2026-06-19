using FizzBuzz;
using FizzBuzz.Presentation;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(options => options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0);
builder.Services.AddProblemDetails();
builder.Services.AddFizzBuzz();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "FizzBuzz API"));
}

app.UseHttpsRedirection();

app.MapFizzBuzzEndpoints();
app.MapStatisticsEndpoints();

app.Run();

using System.Net;
using Shouldly;

namespace FizzBuzz.Test.Authorization;

public sealed class AuthorizationEndpointsTests(AuthTestWebApplicationFactory factory)
    : IClassFixture<AuthTestWebApplicationFactory>
{
    private const string FizzBuzzUrl = "/api/v1/fizzbuzz?int1=3&int2=5&limit=5&str1=fizz&str2=buzz";
    private const string StatisticsUrl = "/api/v1/statistics";

    private HttpClient CreateClient(string? roles)
    {
        var client = factory.CreateClient();
        if (roles is not null)
        {
            client.DefaultRequestHeaders.Add(TestAuthHandler.RolesHeader, roles);
        }

        return client;
    }

    [Fact]
    public async Task Should_return_unauthorized_when_calling_fizzbuzz_without_authentication()
    {
        var response = await CreateClient(roles: null).GetAsync(FizzBuzzUrl);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_return_ok_when_calling_fizzbuzz_as_user()
    {
        var response = await CreateClient(KeycloakRoleNames.User).GetAsync(FizzBuzzUrl);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_return_unauthorized_when_calling_statistics_without_authentication()
    {
        var response = await CreateClient(roles: null).GetAsync(StatisticsUrl);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_return_forbidden_when_calling_statistics_as_user_without_admin()
    {
        var response = await CreateClient(KeycloakRoleNames.User).GetAsync(StatisticsUrl);

        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Should_return_ok_when_calling_statistics_as_admin()
    {
        var response = await CreateClient($"{KeycloakRoleNames.User},{KeycloakRoleNames.Admin}").GetAsync(StatisticsUrl);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    private static class KeycloakRoleNames
    {
        public const string User = "fizzbuzz.user";
        public const string Admin = "fizzbuzz.admin";
    }
}

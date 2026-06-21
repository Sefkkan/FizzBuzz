using FizzBuzz.Application;
using FizzBuzz.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FizzBuzz.Test.Authorization;

/// <summary>
/// Boots the real application but swaps Keycloak JWT validation for a deterministic
/// test scheme and the Redis-backed statistics store for an in-memory one.
/// </summary>
public sealed class AuthTestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // The last AddAuthentication call wins for the default scheme, so policies
            // (which use the default scheme) authenticate through TestAuthHandler.
            services.AddAuthentication(TestAuthHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });

            // Keep the tests off any real Redis instance.
            services.RemoveAll<IFizzBuzzStatisticsRepository>();
            services.AddSingleton<IFizzBuzzStatisticsRepository, InMemoryFizzBuzzStatisticsRepository>();
        });
    }
}

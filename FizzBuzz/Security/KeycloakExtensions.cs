using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace FizzBuzz.Security;

/// <summary>
/// Realm roles defined in Keycloak. They gate access to the API endpoints.
/// </summary>
public static class KeycloakRoles
{
    public const string User = "fizzbuzz.user";
    public const string Admin = "fizzbuzz.admin";
}

/// <summary>
/// Authorization policy names applied to the endpoints.
/// </summary>
public static class AuthorizationPolicies
{
    public const string FizzBuzzUser = "FizzBuzzUser";
    public const string StatisticsAdmin = "StatisticsAdmin";
}

public static class KeycloakExtensions
{
    /// <summary>
    /// Flat claim type used for roles, both by the JWT validation and the test scheme.
    /// </summary>
    public const string RoleClaimType = "role";

    /// <summary>
    /// Registers JWT bearer authentication validating tokens issued by Keycloak.
    /// </summary>
    public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("Keycloak");
        var authority = section["Authority"]
                        ?? throw new InvalidOperationException("Missing 'Keycloak:Authority' configuration.");
        var metadataAddress = section["MetadataAddress"]
                              ?? $"{authority}/.well-known/openid-configuration";
        var audience = section["Audience"]
                       ?? throw new InvalidOperationException("Missing 'Keycloak:Audience' configuration.");
        var requireHttpsMetadata = section.GetValue("RequireHttpsMetadata", true);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Tokens are minted for the browser-facing issuer (e.g. http://localhost:8081),
                // but inside the Docker network the API reaches Keycloak through a different host
                // (http://keycloak:8080). MetadataAddress is the backchannel discovery URL, while
                // ValidIssuer is the public issuer baked into the token.
                options.MetadataAddress = metadataAddress;
                options.Authority = authority;
                options.Audience = audience;
                options.RequireHttpsMetadata = requireHttpsMetadata;
                options.MapInboundClaims = false;

                options.TokenValidationParameters.ValidIssuer = authority;
                options.TokenValidationParameters.ValidateIssuer = true;
                options.TokenValidationParameters.ValidateAudience = true;
                options.TokenValidationParameters.RoleClaimType = RoleClaimType;
                options.TokenValidationParameters.NameClaimType = "preferred_username";
            });

        services.AddSingleton<IClaimsTransformation, KeycloakRolesClaimsTransformation>();

        return services;
    }

    /// <summary>
    /// Registers the role-based authorization policies applied to the endpoints.
    /// </summary>
    public static IServiceCollection AddFizzBuzzAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(AuthorizationPolicies.FizzBuzzUser, policy => policy.RequireRole(KeycloakRoles.User))
            .AddPolicy(AuthorizationPolicies.StatisticsAdmin, policy => policy.RequireRole(KeycloakRoles.Admin));

        return services;
    }
}

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace FizzBuzz.Security;

/// <summary>
/// Declares the Keycloak OAuth2 Authorization Code (+ PKCE) security scheme in the
/// generated OpenAPI document, so the Swagger UI exposes a working "Authorize" button.
/// </summary>
public class OAuthSecuritySchemeTransformer(IConfiguration configuration) : IOpenApiDocumentTransformer
{
    private const string SchemeId = "keycloak";

    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var section = configuration.GetSection("Keycloak");
        var authorizationUrl = section["AuthorizationUrl"]
                               ?? throw new InvalidOperationException("Missing 'Keycloak:AuthorizationUrl' configuration.");
        var tokenUrl = section["TokenUrl"]
                       ?? throw new InvalidOperationException("Missing 'Keycloak:TokenUrl' configuration.");

        var scheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri(authorizationUrl),
                    TokenUrl = new Uri(tokenUrl),
                    Scopes = new Dictionary<string, string>
                    {
                        ["openid"] = "OpenID Connect",
                        ["profile"] = "User profile"
                    }
                }
            }
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes[SchemeId] = scheme;

        document.Security ??= new List<OpenApiSecurityRequirement>();
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference(SchemeId, document)] = new List<string> { "openid", "profile" }
        });

        return Task.CompletedTask;
    }
}

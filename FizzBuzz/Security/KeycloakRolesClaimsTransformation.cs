using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace FizzBuzz.Security;

/// <summary>
/// Keycloak nests realm roles under the "realm_access" claim as a JSON object
/// (e.g. {"roles":["fizzbuzz.user"]}). ASP.NET Core role checks expect flat role
/// claims, so we project those roles into individual claims of type <c>role</c>.
/// </summary>
public class KeycloakRolesClaimsTransformation : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var realmAccess = principal.FindFirst("realm_access")?.Value;
        if (string.IsNullOrEmpty(realmAccess))
        {
            return Task.FromResult(principal);
        }

        using var document = JsonDocument.Parse(realmAccess);
        if (!document.RootElement.TryGetProperty("roles", out var roles) || roles.ValueKind != JsonValueKind.Array)
        {
            return Task.FromResult(principal);
        }
        
        var identity = principal.Identities.FirstOrDefault(i => i.RoleClaimType == KeycloakExtensions.RoleClaimType)
                       ?? new ClaimsIdentity(authenticationType: null, nameType: null, roleType: KeycloakExtensions.RoleClaimType);

        foreach (var role in roles.EnumerateArray())
        {
            var value = role.GetString();
            if (value is not null && !identity.HasClaim(KeycloakExtensions.RoleClaimType, value))
            {
                identity.AddClaim(new Claim(KeycloakExtensions.RoleClaimType, value));
            }
        }

        if (!principal.Identities.Contains(identity))
        {
            principal.AddIdentity(identity);
        }

        return Task.FromResult(principal);
    }
}

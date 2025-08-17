using System.Security.Claims;
using AppRoles.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;

namespace AppRoles;

public class RoleClaimsTransformation(AppRolesOptions appRolesOptions) : IClaimsTransformation {
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var authId = principal.Identities.FirstOrDefault(i => i.IsAuthenticated);
        if (authId == null)
            return Task.FromResult(principal);

        var roleIdentity = principal.Identities.FirstOrDefault(i => i.RoleClaimType == ClaimTypes.Role);

        var created = false;
        if (roleIdentity == null)
        {
            roleIdentity = new ClaimsIdentity(
                authenticationType: appRolesOptions.IdentityName,
                nameType: authId.NameClaimType,
                roleType: ClaimTypes.Role
            );
            if (!string.IsNullOrEmpty(authId.Name))
                roleIdentity.AddClaim(new Claim(authId.NameClaimType, authId.Name));
            created = true;
        }

        var username = authId.Name;
        foreach (var role in appRolesOptions.ConfigurationSection.GetChildren())
        {
            var users = role.Get<List<string>>();
            if (users?.Contains(username, StringComparer.OrdinalIgnoreCase) != true) continue;
            
            if (!roleIdentity.HasClaim(ClaimTypes.Role, role.Key))
                roleIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Key));
        }

        if (!created) return Task.FromResult(principal);
        
        var identities = principal.Identities.ToList();
        identities.Add(roleIdentity);
        return Task.FromResult(new ClaimsPrincipal(identities));

    }
}
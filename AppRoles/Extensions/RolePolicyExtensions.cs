using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace AppRoles.Extensions;

internal static class RolePolicyExtensions {
    internal static void AddRolePoliciesFromConfig(this AuthorizationOptions options, IConfiguration config) {
        var rolesSection = config.GetSection("Roles");
        foreach (var role in rolesSection.GetChildren()) {
            options.AddPolicy(role.Key, policy => policy.RequireClaim(ClaimTypes.Role, role.Key));
        }
    }
}
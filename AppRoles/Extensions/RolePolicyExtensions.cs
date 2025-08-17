using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace AppRoles.Extensions;

internal static class RolePolicyExtensions {
    internal static void AddRolePoliciesFromConfig(this AuthorizationOptions options, IConfigurationSection configSection) {
        foreach (var role in configSection.GetChildren()) {
            options.AddPolicy(role.Key, policy => policy.RequireClaim(ClaimTypes.Role, role.Key));
        }
    }
}
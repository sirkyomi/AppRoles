using AppRoles.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.Extensions.DependencyInjection;

namespace AppRoles.Extensions;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddAppRoles(this IServiceCollection services, Action<AppRolesOptions> options) {
        var optionsInstance = new AppRolesOptions();
        options.Invoke(optionsInstance);
        services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
        services.AddAuthorization(authOptions => authOptions.AddRolePoliciesFromConfig(optionsInstance.Configuration));
        services.AddSingleton(optionsInstance);
        services.AddTransient<IClaimsTransformation, RoleClaimsTransformation>();
        return services;
    }
}
using Microsoft.AspNetCore.Builder;

namespace AppRoles.Extensions;

public static class ApplicationBuilderExtensions {
    public static IApplicationBuilder UseAppRoles(this IApplicationBuilder app) {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}
using Microsoft.Extensions.Configuration;

namespace AppRoles.Models;

public class AppRolesOptions {
    public string AppsettingsSection { get; set; } = "Roles";
    public string IdentityName { get; set; } = "AppRoles-Identity";
    public IConfiguration Configuration { get; set; } = null!;
}
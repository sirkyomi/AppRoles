using Microsoft.Extensions.Configuration;

namespace AppRoles.Models;

public class AppRolesOptions {
    public string IdentityName { get; set; } = "AppRoles-Identity";
    public IConfigurationSection ConfigurationSection { get; set; } = null!;
}
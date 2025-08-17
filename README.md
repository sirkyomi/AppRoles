# AppRoles

AppRoles ermöglicht das zentrale Definieren von Benutzerrollen in der `appsettings.json` und erstellt für jeden authentifizierten Benutzer automatisch eine Identity mit allen konfigurierten Rollen. Die Authentifizierung läuft über Windows Auth, wodurch sich die Lösung optimal für interne Unternehmensanwendungen eignet.

## Übersicht

Mit AppRoles können Rollen flexibel und zentral in der Konfigurationsdatei (`appsettings.json`) gepflegt werden. Jeder Benutzer, der sich per Windows Auth anmeldet, erhält automatisch alle in der Konfiguration hinterlegten Rollen. Das erleichtert die Verwaltung und das Rollout von Berechtigungen.

## Features

- Zentrale Rollenverwaltung per `appsettings.json`
- Automatische Zuordnung aller Rollen bei Anmeldung
- Identity-Objekt enthält alle konfigurierten Rollen
- Windows Authentifizierung (Integrated Windows Authentication)
- Einfache Integration in .NET 8-Anwendungen
- Bereitstellung als NuGet-Paket

## Technologie & Architektur

- .NET 8
- `appsettings.json` als Konfigurationsquelle für Rollen
- Windows Authentication (IIS, Kestrel, Self-Hosting)
- Rollen werden bei jedem Login ausgelesen und der User-Identity zugeordnet

## Projektstruktur

```
.
├─ src/                 # Quellcode
├─ appsettings.json     # Rollen-Konfiguration
├─ Program.cs           # Einstiegspunkt
├─ README.md
└─ ...
```

## Voraussetzungen

- .NET 8 SDK
- Windows-Umgebung (für Windows Auth)
- IIS oder Kestrel

## Schnellstart

1. NuGet-Paket installieren
```bash
dotnet add package AppRoles
```

2. Rollen in der `appsettings.json` konfigurieren:
    ```json
    {
      "Roles": {
        "Admin": ["Alice", "Bob"],
        "Editor": ["Charlie", "Dana"],
        "Viewer": ["Eve"]
      }
    }
    ```
    Domain-Umgebungen:
   ```json
    {
      "Roles": {
        "Admin": ["DOMAIN\\Alice", "DOMAIN\\Bob"],
        "Editor": ["DOMAIN\\Charlie", "DOMAIN\\Dana"],
        "Viewer": ["DOMAIN\\Eve"]
      }
    }
    ```
3. Anwendung konfigurieren:
   ```csharp
   builder.Services.AddAppRoles(options => {
    options.AppsettingsSection = "Roles"; //Appsettings-Key
    options.IdentityName = "AppRoles-Identity"; //Identity-Name für die neue Identity
    options.Configuration = builder.Configuration; //Konfiguration, aus der ausgelesen werden soll
   });
   
   ...

   app.UseAppRoles();
   ```

4. Anwendung starten:
    ```bash
    dotnet restore
    dotnet build
    dotnet run
    ```

4. Anmeldung erfolgt automatisch via Windows Auth. Jeder angemeldete User erhält alle in `Roles` definierten Rollen (basierend auf seinem Namen).

## Beispiele

### Beispiel 1: ASP.NET Core MVC (.NET 8)

**1. NuGet-Paket installieren**
```bash
dotnet add package AppRoles
```

**2. Rollen in `appsettings.json` definieren**
```json
{
  "Roles": {
    "Admin": ["Alice", "Bob"],
    "Editor": ["Charlie", "Dana"],
    "Viewer": ["Eve"]
  }
}
```

**3. In `Program.cs` integrieren**
```csharp
using AppRoles.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppRoles(options => {
    options.AppsettingsSection = "Roles"; //Appsettings-Key
    options.IdentityName = "AppRoles-Identity"; //Identity-Name für die neue Identity
    options.Configuration = builder.Configuration; //Konfiguration, aus der ausgelesen werden soll
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAppRoles(); //<-- Muss nach app.UseRouting() und vor app.MapControllers() aufgerufen werden

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

### Beispiel 2: Minimal API (.NET 8)

**1. NuGet-Paket installieren**
```bash
dotnet add package AppRoles
```

**2. Rollen in `appsettings.json` definieren**  
(siehe oben)

**3. In `Program.cs` integrieren**
```csharp
using AppRoles.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppRoles(options => {
    options.AppsettingsSection = "Roles"; //Appsettings-Key
    options.IdentityName = "AppRoles-Identity"; //Identity-Name für die neue Identity
    options.Configuration = builder.Configuration; //Konfiguration, aus der ausgelesen werden soll
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
}

app.UseAppRoles(); <-- Diese Zeile ist neu
app.UseHttpsRedirection();
```

---

## Hinweise zur Nutzung

- Die Rollen werden bei jedem Login aus der Konfiguration ausgelesen und dem angemeldeten Benutzer als Claims hinzugefügt.
- Windows Auth wird unterstützt, aber das Paket kann auch mit anderen Authentifizierungsmethoden kombiniert werden.
- Die Claims können im Code bequem aus `ClaimsPrincipal` ausgelesen werden.

## Weiterführende Links

- [NuGet Gallery – AppRoles](https://www.nuget.org/packages/AppRoles)

## Lizenz

MIT – siehe [LICENSE](LICENSE).

## Kontakt

- Autor: [@sirkyomi](https://github.com/sirkyomi)
- Issues: https://github.com/sirkyomi/AppRoles/issues

---

**Hinweis:**  
Die Lösung ist für Windows Auth konzipiert.

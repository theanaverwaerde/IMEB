using IMEB.Web;
using IMEB.Web.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

builder.Services.AddHttpContextAccessor()
    .AddTransient<AuthorizationHandler>();

builder.Services.AddHttpClient<WeatherApiClient>(client => client.BaseAddress = new Uri("https+http://apiservice"))
    .AddHttpMessageHandler<AuthorizationHandler>();

string? realm = builder.Configuration.GetValue<string>("Realm");
ArgumentException.ThrowIfNullOrEmpty(realm);

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddKeycloakOpenIdConnect(
        "keycloak", 
        realm: realm, 
        OpenIdConnectDefaults.AuthenticationScheme,
        options =>
        {
            options.ClientId = "IMEBWeb";
            options.ResponseType = OpenIdConnectResponseType.Code;
            options.Scope.Add("imeb:all");
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
            options.SaveTokens = true;
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();
app.MapLoginAndLogout();

app.Run();
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ZaposliMe.Frontend.Components;
using ZaposliMe.Frontend.Identity;
using ZaposliMe.Service.Services.Weather;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// register the cookie handler
builder.Services.AddTransient<CookieHandler>();
builder.Services.AddScoped<WeatherForecastService>();

// set up authorization
builder.Services.AddAuthorizationCore();

// register the custom state provider
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();

// register the account management interface
builder.Services.AddScoped(
    sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

// configure client for auth interactions
builder.Services.AddHttpClient(
    "Backend",
    opt => opt.BaseAddress = new Uri("https://localhost:7097"))
    .AddHttpMessageHandler<CookieHandler>();

await builder.Build().RunAsync();

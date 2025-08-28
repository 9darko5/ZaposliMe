using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ZaposliMe.Frontend.Components;
using ZaposliMe.Frontend.Identity;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// register the cookie handler
builder.Services.AddTransient<CookieHandler>();

// set up authorization
builder.Services.AddAuthorizationCore();

// register the custom state provider
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();

// register the account management interface
builder.Services.AddScoped(
    sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

var backendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? "https://localhost:7097/";

// configure client for auth interactions
builder.Services.AddHttpClient(
    "Backend",
    opt => opt.BaseAddress = new Uri(backendUrl))
    .AddHttpMessageHandler<CookieHandler>();

builder.Services.AddScoped(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    return factory.CreateClient("Backend");
});

await builder.Build().RunAsync();

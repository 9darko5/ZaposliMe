using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.Globalization;
using ZaposliMe.Frontend.Components;
using ZaposliMe.Frontend.Identity;       

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//enable localization
builder.Services.AddLocalization();


// register the cookie handler
builder.Services.AddAuthorizationCore();
builder.Services.AddTransient<JwtHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();
builder.Services.AddScoped(sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

var backendUrl = "https://sljakomat-dgg5dhh6djbygkcf.westeurope-01.azurewebsites.net"; //"https://localhost:7097";

// configure client for auth interactions
builder.Services.AddHttpClient("Backend", c => c.BaseAddress = new Uri(backendUrl)).AddHttpMessageHandler<JwtHandler>();
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Backend"));

var host = builder.Build();
var js = host.Services.GetRequiredService<IJSRuntime>();

var saved = await js.InvokeAsync<string?>("localStorage.getItem", "culture");
var cultureName = string.IsNullOrWhiteSpace(saved) ? "en" : saved;

var culture = new CultureInfo(cultureName);
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await js.InvokeVoidAsync("document.documentElement.setAttribute", "lang", culture.Name);
await host.RunAsync();

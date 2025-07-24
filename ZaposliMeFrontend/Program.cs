using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using ZaposliMe.Frontend;
using ZaposliMe.Frontend.Helpers;
using ZaposliMe.Frontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<FetchWithCredentialsHttpHandler>();

builder.Services.AddScoped(sp => {
    var jsRuntime = sp.GetRequiredService<IJSRuntime>();
    var handler = new FetchWithCredentialsHttpHandler(jsRuntime);

    return new HttpClient(handler)
    {
        BaseAddress = new Uri("https://localhost:7097/")
    };
});

// Auth state provider
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthStateProvider>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<WeatherForecastService>();


await builder.Build().RunAsync();

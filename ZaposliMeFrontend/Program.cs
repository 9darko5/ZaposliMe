using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ZaposliMe.Frontend.Components;
using ZaposliMe.Frontend.Identity;

<<<<<<< HEAD
internal class Program
{
    private static async Task Main(string[] args)
=======
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped<FetchWithCredentialsHttpHandler>();

builder.Services.AddScoped(sp => {
    //var jsRuntime = sp.GetRequiredService<IJSRuntime>();
    //var handler = new FetchWithCredentialsHttpHandler(jsRuntime);

    return new HttpClient(/*handler*/)
>>>>>>> 6019270bc2c63e59e470447404f6ff876fb33b88
    {
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

        // set base address for default host
        builder.Services.AddScoped(sp =>
            new HttpClient { BaseAddress = new Uri("https://localhost:7005") });

        // configure client for auth interactions
        builder.Services.AddHttpClient(
            "Auth",
            opt => opt.BaseAddress = new Uri("https://localhost:7097"))
            .AddHttpMessageHandler<CookieHandler>();

        await builder.Build().RunAsync();
    }
}
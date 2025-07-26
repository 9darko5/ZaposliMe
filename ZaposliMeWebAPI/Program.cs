using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZaposliMe.Persistance;
using ZaposliMe.Domain.Entities.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add authentication and authorization
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();
builder.Services.AddAuthorization();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login"; // ✅ correct minimal API login endpoint
    options.AccessDeniedPath = "/access-denied"; // optional
    options.Events.OnRedirectToLogin = context =>
    {
        // Return 401 to Blazor instead of redirecting
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});

builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<UserManagementDbContext>()
    .AddApiEndpoints();

builder.Services.AddDbContext<UserManagementDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("IdentityConnection"),
        b => b.MigrationsAssembly("ZaposliMe.WebAPI"));
});

builder.Services.AddDbContext<ZaposliMeDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ZaposliMeConnection"),
        b => b.MigrationsAssembly("ZaposliMe.WebAPI"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor",
        policy => policy
            .WithOrigins("https://localhost:7005") // your Blazor WASM client port
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()); // MUST for cookies
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.ApplyMigrations();
app.UseHttpsRedirection();

app.UseCors("AllowBlazor");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapIdentityApi<User>();

app.MapPost("/logout", async (HttpContext httpContext, SignInManager<User> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.NoContent();
});

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    if (response.StatusCode == 403)
    {
        await response.WriteAsync("Access denied: You are not in the required role.");
    }
    else if (response.StatusCode == 404)
    {
        await response.WriteAsync("Resource not found.");
    }
});

app.Run();

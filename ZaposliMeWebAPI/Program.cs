using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZaposliMe.Application.Queries.User.GetUserByEmail;
using ZaposliMe.Domain;
using ZaposliMe.Domain.Entities.Identity;
using ZaposliMe.Domain.Generic;
using ZaposliMe.Domain.Interfaces;
using ZaposliMe.Domain.Repository;
using ZaposliMe.Persistance;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("Final Connection String: " + builder.Configuration.GetConnectionString("ZaposliMeConnection"));

foreach (var kv in builder.Configuration.AsEnumerable())
{
    if (kv.Key.Contains("ConnectionStrings"))
    {
        Console.WriteLine($"{kv.Key} = {kv.Value}");
    }
}
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
    options.LoginPath = "/account/login"; // ✅ correct minimal API login endpoint
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


var dddd = builder.Environment.EnvironmentName;
var ddd = builder.Configuration.GetConnectionString("ZaposliMeConnection");
builder.Services.AddDbContext<UserManagementDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ZaposliMeConnection"),
        b => b.MigrationsAssembly("ZaposliMe.WebAPI"));
});

builder.Services.AddDbContext<ZaposliMeDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ZaposliMeConnection"),
        b => b.MigrationsAssembly("ZaposliMe.WebAPI"));
});


builder.Services.AddDbContext<ZaposliMeQueryDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ZaposliMeConnection"),
        b => b.MigrationsAssembly("ZaposliMe.WebAPI"));
});

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    configuration.RegisterServicesFromAssemblies(typeof(GetUserByEmailQuery).Assembly);
});

var frontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? "https://localhost:7005/";

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor",
        policy => policy
            .WithOrigins(frontendUrl) // your Blazor WASM client port
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()); // MUST for cookies
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//app.ApplyMigrations();
app.UseHttpsRedirection();

app.UseCors("AllowBlazor");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//app.MapPost("account/logout", async (HttpContext httpContext, SignInManager<User> signInManager) =>
//{
//    await signInManager.SignOutAsync();
//    return Results.NoContent();
//});

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

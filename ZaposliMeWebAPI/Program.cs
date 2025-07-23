using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZaposliMe.Domain;
using ZaposliMe.Domain.Entities.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add authentication and authorization
<<<<<<< HEAD

// establish cookie authentication
=======
>>>>>>> 6019270bc2c63e59e470447404f6ff876fb33b88
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();

builder.Services.AddAuthorization();

builder.Services.AddIdentityCore<User>()
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

//app.MapPost("/logout", async (SignInManager<User> signInManager, [FromBody] object empty) =>
//{
//    if (empty is not null)
//    {
//        await signInManager.SignOutAsync();

//        return Results.Ok();
//    }

//    return Results.Unauthorized();
//}).RequireAuthorization();

//app.ApplyMigrations();
app.UseHttpsRedirection();

//// provide an endpoint for user roles
//app.MapGet("/roles", (ClaimsPrincipal user) =>
//{
//    if (user.Identity is not null && user.Identity.IsAuthenticated)
//    {
//        var identity = (ClaimsIdentity)user.Identity;
//        var roles = identity.FindAll(identity.RoleClaimType)
//            .Select(c =>
//                new
//                {
//                    c.Issuer,
//                    c.OriginalIssuer,
//                    c.Type,
//                    c.Value,
//                    c.ValueType
//                });

//        return TypedResults.Json(roles);
//    }

//    return Results.Unauthorized();
//}).RequireAuthorization();

//// provide an endpoint example that requires authorization
//app.MapPost("/data-processing-1", ([FromBody] FormModel model) =>
//    Results.Text($"{model.Message.Length} characters"))
//        .RequireAuthorization();

//// provide an endpoint example that requires authorization with a policy
//app.MapPost("/data-processing-2", ([FromBody] FormModel model) =>
//    Results.Text($"{model.Message.Length} characters"))
//        .RequireAuthorization(policy => policy.RequireRole("Manager"));

app.UseCors("AllowBlazor");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapIdentityApi<User>();

app.Run();


// example form model
class FormModel
{
    public string Message { get; set; } = string.Empty;
}
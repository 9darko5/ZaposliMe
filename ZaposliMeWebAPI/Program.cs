using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using ZaposliMe.Application.DTOs.Account;
using ZaposliMe.Application.Queries.User.GetUserByEmail;
using ZaposliMe.Domain;
using ZaposliMe.Domain.Entities.Identity;
using ZaposliMe.Domain.Generic;
using ZaposliMe.Domain.Interfaces;
using ZaposliMe.Domain.Repository;
using ZaposliMe.Persistance;
using ZaposliMe.WebAPI.Controllers;
using ZaposliMe.WebAPI.Validators;


var builder = WebApplication.CreateBuilder(args);

foreach (var kv in builder.Configuration.AsEnumerable())
{
    if (kv.Key.Contains("ConnectionStrings"))
    {
        Console.WriteLine($"{kv.Key} = {kv.Value}");
    }
}
// Add services to the container.
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add authentication and authorization
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JWT"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwt = builder.Configuration.GetSection("JWT").Get<JwtOptions>() ?? new();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = jwt.Issuer,
        ValidAudience = jwt.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});
builder.Services.AddAuthorization();

builder.Services.AddIdentityCore<User>(options =>
    {
        options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
    })
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

var frontendUrl = "https://www.sljakomat.org"; //"https://localhost:7005"; 

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor",
        policy => policy
            .WithOrigins(frontendUrl) 
            .AllowAnyHeader()
            .AllowAnyMethod()); 
});

builder.Services.AddRateLimiter(opts =>
{
    opts.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
    {
        var key = ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: key,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,              
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0                  
            });
    });

    opts.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IEmployerReviewRepository, EmployerReviewRepository>();

//validators
builder.Services.AddScoped<IValidator<RegisterDto>, RegisterDtoValidator>();

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

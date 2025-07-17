using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZaposliMe.Domain;
using ZaposliMe.Domain.Entities.Identity;
using ZaposliMe.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add authentication and authorization
builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);

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

//app.ApplyMigrations();
app.UseCors("AllowBlazor");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapIdentityApi<User>();

app.Run();

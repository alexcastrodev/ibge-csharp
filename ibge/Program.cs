using System.Text;
using ibge.Repositories;
using ibge.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "IBGE API",
        Description = "Challenge of https://balta.io",
        Version = "v1"
    });
});

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();


var databaseServer = Environment.GetEnvironmentVariable("AZURE_DB_URL") ?? "";
var databaseUser = Environment.GetEnvironmentVariable("AZURE_DB_USER") ?? "";
var databasePassword = Environment.GetEnvironmentVariable("AZURE_DB_PASSWORD") ?? "";
var databaseName = Environment.GetEnvironmentVariable("AZURE_DB_DB") ?? "";

var conn =
    $"Server={databaseServer},1433;Database=${databaseName};User={databaseUser};Password={databasePassword};TrustServerCertificate=true;";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(conn)
);


builder.Services.AddSingleton<IConfiguration>(configuration);

builder.Services.AddScoped<IUserRepository, UserService>();
builder.Services.AddScoped<ILocationRepository, LocationService>();

var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"] ?? "");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "IBGE API V1"); });

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
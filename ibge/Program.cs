using System.Text;
using ibge.Repository;
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

var conn = builder.Configuration["SqlServer:DefaultConnection"];
var configuration = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json")
	.Build();


builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(conn)
);


builder.Services.AddSingleton<IConfiguration>(configuration);

builder.Services.AddScoped<IUserRepository, UserService>();

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
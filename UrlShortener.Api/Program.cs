using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using UrlShortener.Api.Constants;
using UrlShortener.Api.Filters;
using UrlShortener.Api.StartupExtensions;
using UrlShortener.Domain.Constants;
using UrlShortener.Domain.DbContexts;
using UrlShortener.Domain;
using System.IdentityModel.Tokens.Jwt;
using UrlShortener.Domain.Algorithm;
using UrlShortener.Domain.Repositories;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerConfiguration();

builder.Services.AddControllers();

builder.Services.AddDbContext<UrlShortenerDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDomainServices(
    builder.Configuration.GetConnectionString("AppDbConnection"),
    builder.Configuration.GetSection("Jwt")
);

builder.Services
    .AddAuthorizationBuilder()
    .AddPolicy(AuthorizePolicies.Admin, policy => policy.RequireClaim(ClaimTypes.Role, Roles.Admin))
    .AddPolicy(AuthorizePolicies.User, policy => policy.RequireClaim(ClaimTypes.Role, Roles.User));


builder.Services
    .AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
{
    var jwtConfig = builder.Configuration.GetSection("Jwt");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig["Issuer"],
        ValidAudience = jwtConfig["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Secret"])),
        RoleClaimType = ClaimNames.Role,
        NameClaimType = JwtRegisteredClaimNames.Name
    };
});

ServiceValidatorConfiguration.AddValidatorConfiguration(builder.Services);

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IUrlShortenerAlgoritm, UrlShortenerAlgoritm>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RequestAuditMiddleware>();

app.MapControllers();


await app.RunAsync();

public partial class Program { }
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using UrlShortener.Domain.Commands;
using UrlShortener.Domain.Configuration;
using UrlShortener.Domain.DbContexts;

namespace UrlShortener.Domain;

public static class DomainServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services,
        string connectionString,
        IConfiguration jwt)
    {
        services
            .AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
            })
            .AddEntityFrameworkStores<UrlShortenerDbContext>()
            .AddDefaultTokenProviders();

        return services
        .AddOptions()
            .Configure<JwtSettings>(jwt)
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateShortUrlCommand>())
            .AddDbContext<UrlShortenerDbContext>(options => options.UseNpgsql(connectionString));
    }
}
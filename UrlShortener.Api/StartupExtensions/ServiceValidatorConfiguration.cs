using FluentValidation;

using UrlShortener.Api.Validator;
using UrlShortener.Contracts.Http;

namespace UrlShortener.Api.StartupExtensions;

public static class ServiceValidatorConfiguration
{
    public static IServiceCollection AddValidatorConfiguration(this IServiceCollection services)
    {
        return services.AddScoped<IValidator<RegisterUserRequest>, RegisterUserRequestValidator>()
                        .AddScoped<IValidator<AuthenticateUserRequest>, AuthenticateUserRequestValidator>()
                        .AddScoped<IValidator<AdminUpdateUserRequest>, AdminUpdateUserRequestValidator>()
                        .AddScoped<IValidator<AuditRequest>, AuditRequestValidator>()
                        .AddScoped<IValidator<GetShortUrlRequest>, GetUrlValidator>()
                        .AddScoped<IValidator<CreateShortUrlRequest>, UrlShortenerCreateValidator>();
    }
}
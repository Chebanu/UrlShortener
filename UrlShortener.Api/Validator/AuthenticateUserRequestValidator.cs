using FluentValidation;

using UrlShortener.Api.Extensions;
using UrlShortener.Contracts.Http;

internal class AuthenticateUserRequestValidator : AbstractValidator<AuthenticateUserRequest>
{
    public AuthenticateUserRequestValidator()
    {
        this.RuleForUsername(x => x.Username);
        this.RuleForPassword(x => x.Password);
    }
}
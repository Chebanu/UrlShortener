using FluentValidation;

using UrlShortener.Api.Extensions;
using UrlShortener.Contracts.Http;

namespace UrlShortener.Api.Validator;

internal class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        this.RuleForUsername(x => x.Username);
        this.RuleForPassword(x => x.Password);
    }
}
using FluentValidation;
using System.Linq.Expressions;

namespace UrlShortener.Api.Extensions;

internal static class AbstractValidatorUrlExtensions
{
    public const string AllowedUrlCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static void RuleForUrl<T>(this AbstractValidator<T> validator, Expression<Func<T, string>> expression)
    {
        validator.RuleFor(expression)
            .NotEmpty()
            .Length(7)
            .Must(x => x == null || x.All(c => AllowedUrlCharacters.Contains(c)))
            .WithMessage($"Url must only contain the following characters: {AllowedUrlCharacters}");
    }
}

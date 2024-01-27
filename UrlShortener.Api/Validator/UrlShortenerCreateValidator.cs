using FluentValidation;
using UrlShortener.Contracts.Http;

namespace UrlShortener.Api.Validator;

public class UrlShortenerCreateValidator : AbstractValidator<CreateShortUrlRequest>
{
    public UrlShortenerCreateValidator()
    {
        RuleFor(x => x.OriginUrl)
            .Must(StartWithHttp)
            .WithMessage("Link must starts with 'http' or 'https'");
    }

    private bool StartWithHttp(string url)
    {
        return url.StartsWith("http://") || url.StartsWith("https://");
    }
}
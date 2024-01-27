using FluentValidation;
using UrlShortener.Api.Extensions;
using UrlShortener.Contracts.Http;

namespace UrlShortener.Api.Validator;

public class GetUrlValidator : AbstractValidator<GetShortUrlRequest>
{
    public GetUrlValidator()
    {
        this.RuleForUrl(x => x.ModifiedUrl);
    }
}

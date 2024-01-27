using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using UrlShortener.Domain.Commands;
using UrlShortener.Domain.Queries;
using UrlShortener.Contracts.Http;

using FluentValidation;

namespace UrlShortener.Api.Controllers;

[Route("url")]
public class UrlController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreateShortUrlRequest> _createValidator;
    private readonly IValidator<GetShortUrlRequest> _getUrlValidator;

    public UrlController(IMediator mediator,
                        IValidator<CreateShortUrlRequest> createValidator,
                        IValidator<GetShortUrlRequest> getUrlValidator)
    {
        _mediator = mediator;
        _createValidator = createValidator;
        _getUrlValidator = getUrlValidator;
    }

    [HttpGet]
    [Route("{shortUrl}")]
    [ProducesResponseType(typeof(GetOriginUrlByShorlUrlResult), 302)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> GetUrl(string shortUrl, CancellationToken cancellationToken = default)
    {
        var validationResult = await _getUrlValidator.ValidateAsync( new GetShortUrlRequest { ModifiedUrl = shortUrl }, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(new ErrorResponse
            {
                Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray()
            });
        }

        GetOriginUrlByShorlUrlQuery getShortUrl = new()
        {
            ShortUrl = shortUrl
        };

        var result = await _mediator.Send(getShortUrl, cancellationToken);

        return Redirect(result.LongUrl);
    }

    [HttpPost]
    [Route("")]
    [Authorize]
    [ProducesResponseType(typeof(CreateShortUrlResult), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public async Task<IActionResult> CreateUrl([FromBody] CreateShortUrlRequest createShortUrlRequest,
                                                            CancellationToken cancellationToken = default)
    {
        var validationResult = await _createValidator.ValidateAsync(createShortUrlRequest, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(new ErrorResponse
            {
                Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray()
            });
        }

        CreateShortUrlCommand createShortUrlCommand = new()
        {
            OriginUrl = createShortUrlRequest.OriginUrl
        };

        var result = await _mediator.Send(createShortUrlCommand, cancellationToken);

        return Created($"http://localhost:5000/url/{result.ShortUrl}", result);
    }
}
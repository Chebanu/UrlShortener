using MediatR;
using Microsoft.AspNetCore.Mvc;
using PassGuardia.Domain.Commands;
using PassGuardia.Domain.Queries;
using UrlShortener.Contracts.Http;

namespace UrlShortener.Api.Controllers;

[Route("url")]
public class UrlController : ControllerBase
{
    private readonly IMediator _mediator;

    public UrlController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{shortUrl}")]
    public async Task<IActionResult> GetUrl(string shortUrl, CancellationToken cancellationToken = default)
    {
        GetOriginUrlByShorlUrlQuery getShortUrl = new()
        {
            ShortUrl = shortUrl
        };

        var result = await _mediator.Send(getShortUrl, cancellationToken);

        return Redirect(result.LongUrl);
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateUrl([FromBody] CreateShortUrlRequest createShortUrlRequest, CancellationToken cancellationToken = default)
    {
        CreateShortUrlCommand createShortUrlCommand = new()
        {
            OriginUrl = createShortUrlRequest.OriginUrl
        };

        var result = await _mediator.Send(createShortUrlCommand, cancellationToken);

        return Created($"http://localhost:5000/url/{result.ShortUrl}", result);
    }
}
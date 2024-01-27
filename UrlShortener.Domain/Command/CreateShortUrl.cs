using MediatR;

using Microsoft.Extensions.Logging;

using UrlShortener.Contracts.Http;
using UrlShortener.Contracts.Models;
using UrlShortener.Domain.Algorithm;
using UrlShortener.Domain.DbContexts;
using UrlShortener.Domain.Handlers;
using UrlShortener.Domain.Repositories;

namespace UrlShortener.Domain.Commands;

public class CreateShortUrlCommand : IRequest<CreateShortUrlResult>
{
    public string OriginUrl { get; init; }
}
public class CreateShortUrlResult
{
    public string ShortUrl { get; init; }
    public string[] Errors { get; set; }
}
public class CreatePasswordCommandHandler : BaseRequestHandler<CreateShortUrlCommand, CreateShortUrlResult>
{
    private readonly IRepository _repository;
    private readonly IUrlShortenerAlgoritm _urlShortenerAlgoritm;

    public CreatePasswordCommandHandler(IRepository repository,
                                        IUrlShortenerAlgoritm urlShortenerAlgoritm,
                                        ILogger<BaseRequestHandler<CreateShortUrlCommand, CreateShortUrlResult>> logger) : base(logger)
    {
        _repository = repository;
        _urlShortenerAlgoritm = urlShortenerAlgoritm;
    }

    protected override async Task<CreateShortUrlResult> HandleInternal(CreateShortUrlCommand request, CancellationToken cancellationToken)
    {
        string shortedUrl;

        while (true)
        {
            shortedUrl = _urlShortenerAlgoritm.GenerateUniqueUrl();

            var isShortedUrlAlreadyExists = await _repository.GetOriginUrl(new GetShortUrlRequest { ModifiedUrl = shortedUrl }, cancellationToken);

            if (isShortedUrlAlreadyExists == null)
            {
                break;
            }
        }

        var createModel = new UrlShortenerModel
        {
            OriginUrl = request.OriginUrl,
            ModifiedUrl = shortedUrl,
            CreateDate = DateTime.UtcNow
        };

        await _repository.CreateShortUrl(createModel, cancellationToken);

        return new CreateShortUrlResult
        {
            ShortUrl = createModel.ModifiedUrl
        };
    }
}
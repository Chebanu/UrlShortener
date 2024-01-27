using MediatR;

using Microsoft.Extensions.Logging;

using UrlShortener.Contracts.Http;
using UrlShortener.Domain.Handlers;
using UrlShortener.Domain.Repositories;

namespace UrlShortener.Domain.Queries;

public class GetOriginUrlByShorlUrlQuery : IRequest<GetOriginUrlByShorlUrlResult>
{
    public string ShortUrl { get; init; }
}

public class GetOriginUrlByShorlUrlResult
{
    public string LongUrl { get; init; }
}

public class GetOriginUrlByShorlUrlQueryHandler : BaseRequestHandler<GetOriginUrlByShorlUrlQuery, GetOriginUrlByShorlUrlResult>
{
    private readonly IRepository _repository;

    public GetOriginUrlByShorlUrlQueryHandler(IRepository repository,
                ILogger<BaseRequestHandler<GetOriginUrlByShorlUrlQuery, GetOriginUrlByShorlUrlResult>> logger) : base(logger)
    {
        _repository = repository;
    }

    protected override async Task<GetOriginUrlByShorlUrlResult> HandleInternal(GetOriginUrlByShorlUrlQuery request, CancellationToken cancellationToken)
    {
        var req = new GetShortUrlRequest
        {
            ModifiedUrl = request.ShortUrl
        };

        var getDto = await _repository.GetOriginUrl(req, cancellationToken);

        return new GetOriginUrlByShorlUrlResult
        {
            LongUrl = getDto.OriginUrl
        };
    }
}
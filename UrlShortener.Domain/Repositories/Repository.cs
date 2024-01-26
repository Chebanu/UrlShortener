using Microsoft.EntityFrameworkCore;
using UrlShortener.Contracts.Http;
using UrlShortener.Contracts.Models;
using UrlShortener.Domain.DbContexts;

namespace UrlShortener.Domain.Repositories;

public interface IRepository
{
    Task<UrlShortenerModel> CreateShortUrl(UrlShortenerModel request, CancellationToken cancellationToken = default);
    Task<UrlShortenerModel> GetOriginUrl(GetShortUrlRequest request, CancellationToken cancellationToken = default);
}

public class Repository : IRepository
{
    private readonly UrlShortenerDbContext _dbUrl;
    public Repository(UrlShortenerDbContext dbUrl)
    {
        _dbUrl = dbUrl;
    }

    public async Task<UrlShortenerModel> CreateShortUrl(UrlShortenerModel request, CancellationToken cancellationToken = default)
    {
        _ = await _dbUrl.UrlShorteners.AddAsync(request, cancellationToken);
        _ = await _dbUrl.SaveChangesAsync(cancellationToken);

        return request;
    }

    public Task<UrlShortenerModel> GetOriginUrl(GetShortUrlRequest request, CancellationToken cancellationToken = default)
    {
        return _dbUrl.UrlShorteners.SingleOrDefaultAsync(url => url.ModifiedUrl == request.ModifiedUrl, cancellationToken);
    }
}
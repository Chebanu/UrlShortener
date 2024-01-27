using Microsoft.EntityFrameworkCore;
using UrlShortener.Contracts.Http;
using UrlShortener.Contracts.Models;
using UrlShortener.Domain.DbContexts;

namespace UrlShortener.Domain.Repositories;

public interface IRepository
{
    Task<UrlShortenerModel> CreateShortUrl(UrlShortenerModel request, CancellationToken cancellationToken = default);
    Task<UrlShortenerModel> GetOriginUrl(GetShortUrlRequest request, CancellationToken cancellationToken = default);
    Task<Audit> CreateAudit(Audit audit, CancellationToken cancellationToken = default);
    Task<List<Audit>> GetAudits(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
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
    public async Task<Audit> CreateAudit(Audit audit, CancellationToken cancellationToken = default)
    {
        _ = await _dbUrl.Audits.AddAsync(audit, cancellationToken);
        _ = await _dbUrl.SaveChangesAsync(cancellationToken);

        return audit;
    }

    public Task<List<Audit>> GetAudits(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return _dbUrl.Audits
                    .OrderByDescending(desc => desc.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);
    }
}
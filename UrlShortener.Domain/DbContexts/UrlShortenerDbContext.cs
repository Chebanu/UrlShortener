using Microsoft.EntityFrameworkCore;
using UrlShortener.Contracts.Models;

namespace UrlShortener.Domain.DbContexts;

public class UrlShortenerDbContext : DbContext
{
    public DbSet<UrlShortenerModel> UrlShorteners { get; set; }

    public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }
}
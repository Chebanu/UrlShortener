using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Contracts.Models;

namespace UrlShortener.Domain.DbContexts;

public class UrlShortenerDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<UrlShortenerModel> UrlShorteners { get; set; }
    public DbSet<Audit> Audits { get; set; }

    public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }
}
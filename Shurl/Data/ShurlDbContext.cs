using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shurl.Core;
using Shurl.Models;

namespace Shurl.Data;

public class ShurlDbContext(DbContextOptions<ShurlDbContext> options, IGetUserClaims claims) : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<Urls> Urls { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        _ = builder.Entity<Urls>()
            .HasQueryFilter(u => u.UserId == claims.UserId)
            .HasIndex(u => u.ShortUrl)
            .IsUnique();
    }
}

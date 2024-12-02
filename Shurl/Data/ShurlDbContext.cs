using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shurl.Models;

namespace Shurl.Data;

public class ShurlDbContext(DbContextOptions<ShurlDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<Urls> Urls { get; set; }
}

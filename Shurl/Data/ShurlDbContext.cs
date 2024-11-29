using Microsoft.EntityFrameworkCore;
using Shurl.Models;

namespace Shurl.Data
{
    public class ShurlDbContext(DbContextOptions<ShurlDbContext> options) : DbContext(options)
    {
        public DbSet<Urls> Urls { get; set; }
    }
}

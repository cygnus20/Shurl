using Microsoft.EntityFrameworkCore;

namespace Shurl.Models
{
    public record Urls
    {
        public int Id { get; set; }
        public string LongUrl { get; set; } = "";
        public string ShortUrl { get; set; } = "";
    }
}

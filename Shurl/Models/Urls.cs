namespace Shurl.Models
{
    public record Urls
    {
        int Id { get; set; }
        public string LongUrl { get; set; } = "";
        public string ShortUrl { get; set; } = "";
    }
}

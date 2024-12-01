namespace Shurl.Core;

public class UrlService
{
    private readonly HashService _encoderService;
    public UrlService(HashService encoderService)
    {
        this._encoderService = encoderService;
    }
    public string Shorten(int id, string url)
    {
        IEncoder encodeStrat = new Base58Encoder();
        EncoderContext encoderctx = new EncoderContext(encodeStrat);
        string urlHash = _encoderService.ComputeSha256(url)[^8..];
        string idHash = _encoderService.ComputeSha256(id.ToString())[..8];
        string shortUrlHash = $"{idHash}{urlHash}";
        return encoderctx.BaseEncode(shortUrlHash);
    }
}

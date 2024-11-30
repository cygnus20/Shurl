namespace Shurl.Core;

public class UrlService
{
    private readonly HashService _encoderService;
    public UrlService(HashService encoderService)
    {
        this._encoderService = encoderService;
    }
    public string Shorten(string url)
    {
        IEncoder encodeStrat = new Base58Encoder();
        EncoderContext encoderctx = new EncoderContext(encodeStrat);
        string urlHash = _encoderService.ComputeSha256(url);
        return encoderctx.BaseEncode(urlHash);
    }
}

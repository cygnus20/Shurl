namespace Shurl.Core;

public class EncoderContext
{
    private IEncoder _encoder;

    public EncoderContext(IEncoder encoder)
    {
        _encoder = encoder;
    }

    public void SetEncoder(IEncoder encoder)
    {
        _encoder = encoder;
    }

    public string BaseEncode(string value)
    {
        return _encoder.Encode(value);
    }
}

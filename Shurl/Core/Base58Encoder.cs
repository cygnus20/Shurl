using System.Text;

namespace Shurl.Core;

public class Base58Encoder : IEncoder
{
    public string Encode(string value)
    {
        StringBuilder stringBuilder = new StringBuilder();
        const string baseChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ123456789";

        for (int i = 0; i < value.Length - 1; i += 2)
        {
            int index = Convert.ToInt32($"{value[i]}{value[i + 1]}", 16) % baseChars.Length;
            stringBuilder.Append(baseChars[index]);
        }

        return stringBuilder.ToString();
    }
}

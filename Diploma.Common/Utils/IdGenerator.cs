using System.Security.Cryptography;

namespace Diploma.Common.Utils;

public class IdGenerator
{
    public static string GenerateId(int byteSize = 16)
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] data = new byte[byteSize];
            rng.GetBytes(data);
            return Convert.ToBase64String(data)
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');
        }
    }
}
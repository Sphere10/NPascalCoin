using System.IO;
using Org.BouncyCastle.Crypto;

namespace NPascalCoin.Crypto
{
    public interface IKeyParser
    {
        AsymmetricKeyParameter ReadKey(Stream stream);
    }
}
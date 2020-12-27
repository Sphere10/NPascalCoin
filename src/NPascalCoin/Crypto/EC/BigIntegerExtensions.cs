using Org.BouncyCastle.Math;

namespace NPascalCoin.Crypto
{
    internal static class BigIntegerExtensions
    {
        internal static BigInteger Clone(this BigInteger value) => new(value.SignValue, value.ToByteArray());
    }
}
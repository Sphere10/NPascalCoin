using System;

namespace NPascalCoin.Crypto
{
    internal class CryptoException : ApplicationException
    {
        internal CryptoException(string error) : base(error)
        {
        }
    }
}
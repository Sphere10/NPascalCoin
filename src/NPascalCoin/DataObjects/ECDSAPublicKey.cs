using System;

namespace NPascalCoin.DataObjects {
    public class ECDSAPublicKey {
        public UInt16 OpenSSL_NID { get; set; }
        public byte[] X { get; set;}
        public byte[] Y { get; set; }
    }
}
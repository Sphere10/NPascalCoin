using System;
using System.Collections.Generic;
using System.Text;
using NPascalCoin.API.Classic.Objects;
using Org.BouncyCastle.Ocsp;

namespace NPascalCoin.DataObjects {

    public class Account {
        public AccountState State { get; set; }

        public AccountKey Key { get; set; }
        
        public UInt32 LockedUntilBlock { get; set; }

        public UInt64 Price { get; set; }

        public UInt32 AccountToPay { get; set; }

        public AccountKey NewKey { get; set; }

        public byte[] HashLock { get; set; }
    }


}

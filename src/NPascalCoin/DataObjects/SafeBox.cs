using System;
using System.Collections.Generic;

namespace NPascalCoin.DataObjects {

    public class SafeBox {

        public byte[] Root { get; set; }

        public UInt64 TotalBalance { get; set; }

        public UInt64 AccumulatedWork { get; set; }

        public Int32 CurrentProtocol { get; set; }

        public Int32 MaxSafeBoxSnapshots { get; set; }

        public ICollection<AccountSegment> ModifiedBlocksPreviousState { get; set; }

        public ICollection<AccountSegment> ModifiedBlocksFinalState { get; set; }

    }
   
}
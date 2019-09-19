using System;

namespace NPascalCoin.DataObjects {
    public class AccountSegment {
        public BlockHeader SourceBlock { get; set; }
        public Account[] Accounts { get; set; }
        public byte[] BlockHash { get; set; }
        public UInt64 AccumulatedWork { get; set; }
    }
}
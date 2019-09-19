using System;

namespace NPascalCoin.DataObjects {
    public class BlockHeader {
        public UInt32 Number { get; set; }

        public AccountKey MinerKey { get; set; }

        public UInt64 Reward { get; set; }

        public UInt64 Fee { get; set; }
        
        public UInt16 ProtocolVersion { get; set; }
        
        public UInt16 ProtocolAvailable { get; set; }

        public UInt32 TimeStamp { get; set; }

        public UInt32 CompactTarget { get; set; }

        public UInt32 Nonce { get; set; }

        public byte[] Payload { get; set; }

        public byte[] InitialSafeBoxHash { get; set; }

        public byte[] OperationsHash { get; set; }

        public byte[] ProofOfWork { get; set; }
    }


}
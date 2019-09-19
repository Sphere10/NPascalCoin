using System;

namespace NPascalCoin.DataObjects {
    public abstract class Operation {

        public abstract UInt64 AmountTransferred { get; }
    }


}
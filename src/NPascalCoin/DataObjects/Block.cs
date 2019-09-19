using System;
using System.Collections.Generic;

namespace NPascalCoin.DataObjects {
    public class Block {

        public BlockHeader Header { get; set; }

        public IList<Operation> Contents { get; set; }
    }

}
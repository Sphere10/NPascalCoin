using System;
using System.Collections.Generic;
using System.Text;

namespace NPascalCoin.Consensus {
	public class Key {
		public ushort CurveID { get; set; }
		public byte[] X { get; set; }
		public byte[] Y { get; set; }
	}
}
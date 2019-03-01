using System;

namespace NPascalCoin.Common {
	public class PascalCoinException : ApplicationException {
		public PascalCoinException(string error) : base(error)  {			
		}
	}
}

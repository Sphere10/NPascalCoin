using NPascalCoin.API.Classic.Objects;
using NPascalCoin.Common;

namespace NPascalCoin.API.Classic {
	public class PascalCoinRPCException : PascalCoinException {

		public PascalCoinRPCException(string error) : base(error)  {			
		}

		public PascalCoinRPCException(ErrorResult result) : base($"{result.ErrorCode} - {result.Message}") {
			Error = result;
		}

		public ErrorResult Error { get; }

	}
}

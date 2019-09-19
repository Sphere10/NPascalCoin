using System;

namespace NPascalCoin.Operations {
	public static class OperationHelper {
		
		public static string GetOperationID(string ophash) {
			if (ophash == null)
				throw new ArgumentNullException(nameof(ophash));

			if (ophash.Length != Constants.OpHashByteLength*2)
				throw new ArgumentOutOfRangeException(nameof(ophash), "Not a valid OPHASH string");

			return ophash.Substring(8);
		}
	}
}

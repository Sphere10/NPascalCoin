using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace NPascalCoin.API.Classic.Objects {
	public enum ErrorCode {
		InternalError = 100,
		MethodNotFound = 1001,
		InvalidAccount = 1002,
		InvalidBlock = 1003,
		InvalidOperation = 1004,
		InvalidPubKey = 1005,
		NotFound = 1010,
		WalletPasswordProtected = 1015,
		InvalidData = 1016
	}
}

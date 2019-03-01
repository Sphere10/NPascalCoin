using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace NPascalCoin.API.Classic.Objects {

	/// <summary>
	/// Type of payload encoded.
	/// </summary>
	public enum DecryptedPayloadMethod {
		/// <summary>
		/// Payload was decrypted using a key from the wallet
		/// </summary>
		key = 1,


		/// <summary>
		/// Payload was decrypted using a supplied password in the RPC request
		/// </summary>
		pwd
	}

}

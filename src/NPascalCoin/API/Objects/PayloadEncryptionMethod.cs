using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace NPascalCoin.API.Objects {

	/// <summary>
	/// Type of payload encoded.
	/// </summary>
	public enum PayloadEncryptionMethod {

		/// <summary>
		/// Not encoded. Will be visible for everybody
		/// </summary>
		[Description("none")]
		None,

		/// <summary>
		/// Using Public key of "target" account. Only "target" will be able to decrypt this payload
		/// </summary>
		[Description("dest")]
		Dest,

		/// <summary>
		/// Using sender Public key. Only "sender" will be able to decrypt this payload
		/// </summary>
		[Description("sender")]
		Sender,

		/// <summary>
		/// Encrypted data using pwd param
		/// </summary>
		[Description("aes")]
		Aes
	}

}

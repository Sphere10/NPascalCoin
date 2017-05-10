using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace NPascalCoin {

	/// <summary>
	/// Type of key used for public/private key encryption.
	/// </summary>
	public enum KeyType {
		/// <summary>
		/// secp256k1
		/// </summary>
		SECP256K1 = 714,

		/// <summary>
		/// secp384r1
		/// </summary>
		SECP384R1 = 715,

		/// <summary>
		/// secp283k1
		/// </summary>
		SECP283K1 = 729,

		/// <summary>
		/// secp521r1
		/// </summary>
		SECP521R1 = 716,
	}

}

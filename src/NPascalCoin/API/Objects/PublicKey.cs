using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NPascalCoin.API.Objects {

	/// <summary>
	/// A "Public Key object" is a JSON object with information about a public key.
	/// </summary>
	public class PublicKey : PascalCoinDTO {

		/// <summary>
		/// Human readable name stored at the Wallet for this key
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// If false then Wallet doesn't have Private key for this public key, so, Wallet cannot execute operations with this key
		/// </summary>
		[JsonProperty("can_use")]
		public bool CanUse { get; set; }

		/// <summary>
		/// Encoded value of this public key.This HEXASTRING has no checksum, so, if using it always must be sure that value is correct
		/// </summary>
		[JsonProperty("enc_pubkey")]
		public string EncPubKey { get; set; }

		/// <summary>
		/// Encoded value of this public key in Base 58 format, also contains a checksum.This is the same value that Application Wallet exports as a public key
		/// </summary>
		[JsonProperty("b58_pubkey")]
		public string Base58PubKey { get; set; }

		/// <summary>
		/// Indicates which EC type is used (EC_NID)
		/// </summary>
		[JsonProperty("ec_nid")]
		public KeyType KeyType { get; set; }

		/// <summary>
		/// HEXASTRING with x value of public key
		/// </summary>
		[JsonProperty("x")]
		public string X { get; set; }

		/// <summary>
		/// HEXASTRING with y value of public key
		/// </summary>
		[JsonProperty("y")]
		public string Y { get; set; }
	}


}

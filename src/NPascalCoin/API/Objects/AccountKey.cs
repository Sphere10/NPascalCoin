using Newtonsoft.Json;

namespace NPascalCoin.API.Objects {

	/// <summary>
	/// An "Account Key" object is a JSON object that references a key of an account used for signing purposes.
	/// </summary>
	public class AccountKey {

		/// <summary>
		/// Account that will sign
		/// </summary>
		[JsonProperty("account")]
		public int AccountNo { get; set; }

		/// <summary>
		/// Public key that will sign in encoded format.
		/// </summary>
		[JsonProperty("enc_pubkey")]
		public string EncPubKey { get; set; }


		/// <summary>
		/// Public key that will sign in Base 58 format, also contains a checksum.This is the same value that Application Wallet exports as a public key.
		/// </summary>
		[JsonProperty("b58_pubkey")]
		public string Base58PubKey { get; set; }

	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NPascalCoin.DTO {
	/// <summary>
	/// JSON object descibing a decrypted payload 
	/// </summary>
	public class DecryptedPayloadDTO {
		/// <summary>
		///  Decryption result
		/// </summary>
		[JsonProperty("result")]
		public bool Result { get; set; }

		/// <summary>
		/// HEXASTRING - Same value than param payload sent
		/// </summary>
		[JsonProperty("enc_payload")]
		public string OriginalPayload { get; set; }

		/// <summary>
		/// Unencoded value in readable format (no HEXASTRING)
		/// </summary>
		[JsonProperty("unenc_payload")]
		public string UnencryptedPayload { get; set; }

		/// <summary>
		///  HEXASTRING - Unencoded value in hexastring
		/// </summary>
		[JsonProperty("unenc_hexpayload")]
		public string UnencryptedPayloadHex { get; set; }

		/// <summary>
		/// String - "key" or "pwd"
		/// </summary>
		[JsonProperty("payload_method")]
		public PayloadMethod PayloadMethod { get; set; }

		/// <summary>
		/// HEXASTRING - Encoded public key used to decrypt when method = "key"
		/// </summary>
		[JsonProperty("enc_pubkey")]
		public string EncodedPubKey { get; set; }

		/// <summary>
		/// String value used to decrypt when method = "pwd"
		/// </summary>
		[JsonProperty("pwd")]
		public string DecryptPassword { get; set; }
		
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NPascalCoin.API.Classic.Objects {

	/// <summary>
	/// A "Sign Result" is a JSON object which describes the result of a multi-operation signature or verification.
	/// </summary>
	public class SignResult {

		[JsonProperty("digest")]
		public string Digest { get; set; }


		[JsonProperty("enc_pubkey")]
		public string EncPubKey { get; set; }

		[JsonProperty("signature")]
		public string Signature { get; set; }

	}
}

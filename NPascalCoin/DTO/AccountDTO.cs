using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NPascalCoin.DTO {

	/// <summary>
	/// An "Account object" is a JSON object with information about an account.
	/// </summary>
	public class AccountDTO : PascalCoinDTO {

		/// <summary>
		/// Account number
		/// </summary>
		[JsonProperty("account")]
		public uint Account { get; set; }

		/// <summary>
		/// Encoded public key value (hexastring)
		/// </summary>
		[JsonProperty("enc_pubkey")]
		public string EncPubKey { get; set; }

		/// <summary>
		/// Account balance (PASCURRENCY)
		/// </summary>
		[JsonProperty("balance")]
		public decimal Balance { get; set; }

		/// <summary>
		/// Operations made by this account (Note: When an account receives a transaction, n_operation is not changed)
		/// </summary>
		[JsonProperty("n_operation")]
		public uint NumOperations { get; set; }

		/// <summary>
		/// Last block that updated this account. If equal to blockchain blocks count it means that it has pending operations to be included to the blockchain
		/// </summary>
		[JsonProperty("updated_b")]
		public uint LastUpdatedBlock { get; set; }
	}

}

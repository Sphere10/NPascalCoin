using Newtonsoft.Json;

namespace NPascalCoin.API.Objects {

	/// <summary>
	/// An "Account object" is a JSON object with information about an account.
	/// </summary>
	public class Account : PascalCoinDTO {

		/// <summary>
		/// Account number
		/// </summary>
		[JsonProperty("account")]
		public uint AccountNo { get; set; }

		/// <summary>
		/// Account Type
		/// </summary>
		[JsonProperty("type")]
		public ushort Type { get; set; }

		/// <summary>
		/// Account name in PascalCoin64 Encoding - abcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-+{}[]_:"|&lt;>,.?/~
		/// </summary>
		/// <remarks>First char cannot start with number</remarks>
		/// <remarks>Must empty/null or 3..64 characters in length</remarks>
		[JsonProperty("name")]
		public string Name { get; set; }
	
		/// <summary>
		/// Account State 
		/// </summary>
		[JsonProperty("state")]
		public AccountState State { get; set; }

		/// <summary>
		/// For Listed accounts, this indicates whether it's private or public sale
		/// </summary>
		[JsonProperty("private_sale")]
		public bool? PrivateSale { get; set; }

		/// <summary>
		/// For Listed accounts for PrivateSale, this indicates the buyers public key
		/// </summary>
		[JsonProperty("new_enc_pubkey")]
		public string NewEncPubkey { get; set; }

		[JsonProperty("locked_until_block")]
		public uint? LockedUntilBlock { get; set; }

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
		
		/// <summary>
		/// Account price (PASCURRENCY)
		/// </summary>
		[JsonProperty("price")]
		public decimal? Price { get; set; }

		/// <summary>
		/// Seller account number
		/// </summary>
		[JsonProperty("seller_account")]
		public uint? SellerAccount { get; set; }
	}
}

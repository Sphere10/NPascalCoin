using Newtonsoft.Json;
// ReSharper disable InconsistentNaming

namespace NPascalCoin.API.Classic.Objects {

	public class Changer {
		[JsonProperty("account")]
		public int Account { get; set; }

		/// <summary>
		/// N_Operation of Account
		/// </summary>
		[JsonProperty("n_operation")]
		public uint NOperation { get; set; }

		/// <summary>
		/// New public key for account.
		/// </summary>
		[JsonProperty("new_enc_pubkey")]
		public string NewPubKey { get; set; }

		[JsonProperty("new_name")]
		public string NewName { get; set; }

		/// <summary>
		/// N_Operation of Account
		/// </summary>
		[JsonProperty("new_type")]
		public short? NewType { get; set; }

		[JsonProperty("seller_account")]
		public int? SellerAccount { get; set; }

		[JsonProperty("account_price")]
		public decimal? AccountPrice { get; set; }

		[JsonProperty("locked_until_block")]
		public uint? LockedUntilBlock { get; set; }

		[JsonProperty("fee")]
		public decimal? Fee { get; set; }

	}
}
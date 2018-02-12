using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NPascalCoin.DTO {

	/// <summary>
	/// A "Block object" is a JSON object with information about a Blockchain's block. 
	/// </summary>
	public class BlockDTO : PascalCoinDTO {
		
		/// <summary>
		/// Block number
		/// </summary>
		[JsonProperty("block")]
		public uint Block { get; set; }

		/// <summary>
		/// Last block that updated this account. If equal to blockchain blocks count it means that it has pending operations to be included to the blockchain
		/// </summary>
		[JsonProperty("enc_pubkey")]
		public string EncPubKey { get; set; }

		/// <summary>
		/// Reward of first account's block
		/// </summary>
		[JsonProperty("reward")]
		public decimal Reward { get; set; }

		/// <summary>
		/// Fee obtained by operations (PASCURRENCY)
		/// </summary>
		[JsonProperty("fee")]
		public decimal Fee { get; set; }

		/// <summary>
		/// Pascal Coin protocol used
		/// </summary>
		[JsonProperty("ver")]
		public uint Version { get; set; }

		/// <summary>
		/// Pascal Coin protocol available by the miner
		/// </summary>
		[JsonProperty("ver_a")]
		public uint AvailableVersion { get; set; }

		/// <summary>
		/// Unix timestamp
		/// </summary>
		[JsonProperty("timestamp")]
		public uint Timestamp { get; set; }

		/// <summary>
		/// Target used
		/// </summary>
		[JsonProperty("target")]
		public uint CompactTarget { get; set; }

		/// <summary>
		/// Nonce used
		/// </summary>
		[JsonProperty("nonce")]
		public uint Nonce { get; set; }

		/// <summary>
		/// Miner's payload
		/// </summary>
		[JsonProperty("payload")]
		public string Payload { get; set; }

		/// <summary>
		/// SafeBox Hash
		/// </summary>
		[JsonProperty("sbh")]
		public string SafeBoxHash { get; set; }

		/// <summary>
		/// Operations hash
		/// </summary>
		[JsonProperty("oph")]
		public string OperationsHash { get; set; }

		/// <summary>
		/// Proof of work
		/// </summary>
		[JsonProperty("pow")]
		public string ProofOfWork { get; set; }

		/// <summary>
		/// Number of operations included in this block
		/// </summary>
		[JsonProperty("operations")]
		public uint OperationCount { get; set; }

		/// <summary>
		/// Estimated network hashrate calculated by previous 50 blocks average
		/// </summary>
		[JsonProperty("hashratekhs")]
		public ulong Last50HashRateKhs { get; set; }

		/// <summary>
		/// Number of blocks in the blockchain higher than this
		/// </summary>
		[JsonProperty("maturation")]
		public uint Maturation { get; set; }
	}

}

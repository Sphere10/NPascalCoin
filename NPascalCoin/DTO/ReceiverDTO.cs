using Newtonsoft.Json;
using NPascalCoin.Processing;

// ReSharper disable InconsistentNaming

namespace NPascalCoin.DTO {
	public class ReceiverDTO {
		[JsonProperty("account")]
		public int Account { get; set; }

		[JsonProperty("amount")]
		public decimal Amount { get; set; }

		[JsonProperty("payload")]
		[JsonConverter(typeof(HexBinaryConverter))]
		public byte[] Payload { get; set; }
	}
}
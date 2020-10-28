using Newtonsoft.Json;
using NPascalCoin.Extensions.JsonConverters;

// ReSharper disable InconsistentNaming

namespace NPascalCoin.API.Objects {

	public class Receiver {
		[JsonProperty("account")]
		public int Account { get; set; }

		[JsonProperty("amount")]
		public decimal Amount { get; set; }

		[JsonProperty("payload")]
		[JsonConverter(typeof(HexBinaryConverter))]
		public byte[] Payload { get; set; }
	}

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NPascalCoin.Processing {
	public sealed class HexBinaryConverter : JsonConverter {
		public override bool CanConvert(Type objectType) {
			return typeof(byte[]) == objectType;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			writer.WriteValue((value as byte[]).ToHexString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			var str = reader.Value?.ToString();
			if (string.IsNullOrWhiteSpace(str))
				return new byte[0];
			return str.ToByteArray();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using NPascalCoin.API.Classic.Objects;
using NPascalCoin.Common.Parsing;
using Sphere10.Framework;


namespace NPascalCoin.Common {

	public class EPasa {

		public EPasa() : this(PayloadType.NonDeterministic) {
		}

		public EPasa(PayloadType payloadType) {
			PayloadType = payloadType;
		}
		
		public uint? Account { get; set; }

		public uint? AccountChecksum { get; set; }

		public string AccountName { get; set; }

		public PayloadType PayloadType { get; set; }

		public string Payload { get; set; }

		public string Password { get; set; }

		public string ExtendedChecksum { get; set; }

		public byte[] GetRawPayloadBytes() {
			if (PayloadType.HasFlag(PayloadType.AsciiFormatted)) {
				return Encoding.ASCII.GetBytes(Payload);
			} else if (PayloadType.HasFlag(PayloadType.Base58Formatted)) {
				//return Encoding.Base58.GetBytes(Payload);
				throw new NotImplementedException(nameof(PayloadType.Base58Formatted));
			} else if (PayloadType.HasFlag(PayloadType.HexFormatted)) {
				return Payload.ToHexByteArray();
			}
			return Payload.ToHexByteArray();
		}

		public static EPasa Parse(string EPasaText) {
			if (TryParse(EPasaText, out var epasa))
				return epasa;
			throw new ArgumentException("Invalid E-PASA format", nameof(EPasaText));
		}

		public static bool TryParse(string EPasaText, out EPasa epasa) {
			throw new NotImplementedException();
			var EParser = new RecursiveDescentEPasaParser();
			//return EParser.TryParseEPasa(EPasaText, out epasa);		
		}


		public override string ToString() {
			return ToString(false);
		}

		public string ToString(bool omitExtendedChecksum) {
			var result = string.Empty;
			if (PayloadType.HasFlag(PayloadType.AddressedByName)) {
				result += AccountName;
			} else {
				result += Account.ToString();
				if (AccountChecksum != null) {
					result += $"-{AccountChecksum}";
				}
			}

			var payloadContent = string.Empty;
			if (PayloadType.HasFlag(PayloadType.AsciiFormatted)) {
				payloadContent = $@"""{Payload}]""";
			} else if (PayloadType.HasFlag(PayloadType.HexFormatted)) {
				payloadContent = $@"0x{Payload}]";
			} else if (PayloadType.HasFlag(PayloadType.Base58Formatted)) {
				payloadContent = $"{payloadContent}";
			} else {
				// it is non-deterministic, so payload content is ignored
				payloadContent = string.Empty;
			}

			if (PayloadType.HasFlag(PayloadType.Public)) {
				result += $"[{payloadContent}]";
			} else if (PayloadType.HasFlag(PayloadType.RecipientKeyEncrypted)) {
				result += $"({payloadContent})";
			} else if (PayloadType.HasFlag(PayloadType.SenderKeyEncrypted)) {
				result += $"<{payloadContent}>";
			} else if (PayloadType.HasFlag(PayloadType.AESEncrypted)) {
				result += $"{{{payloadContent}:{Password}}}";
			} else {
				// it is non-deterministic, so payload content is never specified
			}

			if (ExtendedChecksum != null && !omitExtendedChecksum)
				result += $":{ExtendedChecksum}";

			return result;
		}
	}
}

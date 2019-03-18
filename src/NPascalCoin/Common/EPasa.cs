using System;
using System.Collections.Generic;
using System.Text;
using NPascalCoin.API.Classic.Objects;
using NPascalCoin.Common.Text;
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
				return System.Text.Encoding.ASCII.GetBytes(Payload);
			}

			if (PayloadType.HasFlag(PayloadType.Base58Formatted)) {
				return PascalBase58Encoding.Decode(Payload);
			}

			if (PayloadType.HasFlag(PayloadType.HexFormatted)) {
				return Payload.ToHexByteArray();
			}
			throw new PascalCoinException("Unknown payload encoding");
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
				result += Pascal64Encoding.Escape(AccountName);
			} else {
				result += Account.ToString();
				if (AccountChecksum != null) {
					result += $"-{AccountChecksum}";
				}
			}

			var payloadContent = string.Empty;
			if (PayloadType.HasFlag(PayloadType.AsciiFormatted)) {
				payloadContent = $@"""{PascalAsciiEncoding.Escape(Payload)}]""";
			} else if (PayloadType.HasFlag(PayloadType.HexFormatted)) {
				payloadContent = $@"0x{Payload}]";
			} else if (PayloadType.HasFlag(PayloadType.Base58Formatted)) {
				payloadContent = $"{Payload}";
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
			} else if (PayloadType.HasFlag(PayloadType.PasswordEncrypted)) {
				result += $"{{{payloadContent}:{PascalAsciiEncoding.Escape(Password)}}}";
			} else {
				// it is non-deterministic, so payload omitted entirely
			}

			if (ExtendedChecksum != null && !omitExtendedChecksum)
				result += $":{ExtendedChecksum}";

			return result;
		}
	}
}

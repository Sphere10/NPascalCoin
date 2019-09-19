using System;
using NPascalCoin.Crypto;
using NPascalCoin.Payloads;
using Sphere10.Framework;

namespace NPascalCoin.Encoding {

	public static class EPasaHelper {
		public const int MaxPublicAsciiContentLength = 255;
		public const int MaxECIESAsciiContentLength = 144;
		public const int MaxAESAsciiContentLength = 223;
		public const int MaxPublicHexContentLength = 510 + 2;
		public const int MaxECIESHexContentLength = 288 + 2;
		public const int MaxAESHexContentLength = 446 + 2;
		public const int MaxPublicBase58ContentLength = 348;
		public const int MaxECIESBase58ContentLength = 196;
		public const int MaxAESBase58ContentLength = 304;
		public const uint ExtendedChecksumMurMur3Seed = 0;

		public static string ComputeExtendedChecksum(string text) {
			if (text == null)
				throw new ArgumentNullException(nameof(text));
			var checksum = (ushort) (Hashers.MURMUR3_32(System.Text.Encoding.ASCII.GetBytes(text), ExtendedChecksumMurMur3Seed) % 65536);
			return EndianBitConverter.Little.GetBytes(checksum).ToHexString(true);
		}

		public static bool IsValidExtendedChecksum(string text, string checksum) {
			var comp = ComputeExtendedChecksum(text);
			return  comp == checksum;
		}

		public static bool IsValidPayloadLength(PayloadType payloadType, string payloadContent) {
			if (string.IsNullOrEmpty(payloadContent))
				return true;

			if (payloadType.HasFlag(PayloadType.Public)) {
				if (payloadType.HasFlag(PayloadType.AsciiFormatted)) {
					return PascalAsciiEncoding.Unescape(payloadContent).Length <= MaxPublicAsciiContentLength;
				}

				if (payloadType.HasFlag(PayloadType.HexFormatted)) {
					return payloadContent.Length <= MaxPublicHexContentLength;
				}

				if (payloadType.HasFlag(PayloadType.Base58Formatted)) {
					return payloadContent.Length <= MaxPublicBase58ContentLength;
				}

				// unknown encoding format
				return false;
			}

			if (payloadType.HasFlag(PayloadType.SenderKeyEncrypted) || payloadType.HasFlag(PayloadType.RecipientKeyEncrypted)) {
				if (payloadType.HasFlag(PayloadType.AsciiFormatted)) {
					return PascalAsciiEncoding.Unescape(payloadContent).Length <= MaxECIESAsciiContentLength;
				}

				if (payloadType.HasFlag(PayloadType.HexFormatted)) {
					return payloadContent.Length <= MaxECIESHexContentLength;
				}

				if (payloadType.HasFlag(PayloadType.Base58Formatted)) {
					return payloadContent.Length <= MaxECIESBase58ContentLength;
				}

				// unknown encoding format
				return false;
			}

			if (payloadType.HasFlag(PayloadType.PasswordEncrypted)) {
				if (payloadType.HasFlag(PayloadType.AsciiFormatted)) {
					return PascalAsciiEncoding.Unescape(payloadContent).Length <= MaxAESAsciiContentLength;
				}

				if (payloadType.HasFlag(PayloadType.HexFormatted)) {
					return payloadContent.Length <= MaxAESHexContentLength;
				}

				if (payloadType.HasFlag(PayloadType.Base58Formatted)) {
					return payloadContent.Length <= MaxAESBase58ContentLength;
				}

				// unknown encoding format
				return false;
			}

			// unknown encryption format
			return false;
		}

		public static bool IsValidPasswordLength(string passwordValue) {
			// no password length policy established (only client-side concern)
			return true;
		}
	}
}

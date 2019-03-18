using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Sphere10.Framework;

namespace NPascalCoin.Common.Text {
	public class HexEncoding {
		public const string HexStringCharSet = "0123456789abcdef";
		public const string HexNibblePattern = @"[0-9a-f]";
		public const string HexBytePattern = HexNibblePattern + "{2}";
		public const string HexStringPattern = "(?:" + HexBytePattern + ")+";
		public const string ExclusiveStringPattern = "(?:" + HexBytePattern + ")+$";

		private static readonly Regex HexStringRegex;

		static HexEncoding() {
			HexStringRegex = new Regex(ExclusiveStringPattern);
		}

		public static bool IsValid(string hexString) {
			return HexStringRegex.IsMatch(hexString);
		}

		public static byte[] Decode(string hexString) {
			if (!TryDecode(hexString, out var result))
				throw new ArgumentException("Invalid hex-formatted string", nameof(hexString));
			return result;
		}

		public static bool TryDecode(string hexString, out byte[] result) {
			result = null;
			if (!IsValid(hexString))
				return false;
			if (hexString.StartsWith("0x"))
				hexString = hexString.Substring(2);
			result = hexString.ToHexByteArray();
			return true;
		}

		public static string Encode(byte[] bytes) {
			return bytes.ToHexString(true).ToLowerInvariant();
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NPascalCoin.Common;
using Sphere10.Framework;

namespace NPascalCoin.Common.Parsing {
	public static class EncodingHelper {
		public const string EPasaEscapeChar = @"\";
		public const string EPasaEscapedChars = @"""():<>[\]{}";		
		public const string SafeAnsiCharSet = @" !""#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
		public const string SafeAnsiCharSetEscaped = @"""():<>[\]{}";
		public const string SafeAnsiCharSetUnescaped = @" !#$%&'*+,-./0123456789;=?@ABCDEFGHIJKLMNOPQRSTUVWXYZ^_`abcdefghijklmnopqrstuvwxyz|~";
		public const string Pascal64CharSet = "abcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-+{}[]_:`|<>,.?/~";
		public const string Pascal64CharSetStart = "abcdefghijklmnopqrstuvwxyz!@#$%^&*()-+{}[]_:`|<>,.?/~";
		public const string Pascal64CharSetEscaped = "(){}[]:<>";
		public const string Pascal64CharSetUnescaped = "abcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*-+_`|,.?/~";
		public const string HexStringCharSet = "0123456789abcdef";
		public const string Base58CharSet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
		public const char EscapeChar = '\\';
		
		public static Regex SafeAnsiStringRegex;
		public static Regex Pascal64Regex;

		static EncodingHelper() {
			SafeAnsiStringRegex = new Regex(RegexEPasaParser.SafeAnsiStringPattern, RegexOptions.Compiled);
			Pascal64Regex = new Regex(RegexEPasaParser.Pascal64StringPattern);
		}

		public static string EscapeUnescapedString(string str) {
			return str
				.Select(c => SafeAnsiCharSetEscaped.Contains(c) ? $@"{EPasaEscapeChar}{c}" : $"{c}")
				.ToDelimittedString(String.Empty);
		}

		public static bool IsValidPascal64String(string str) {
			return Pascal64Regex.IsMatch(str);
		}

		public static bool IsValidUnescapedSafeAnsiString(string str) {
			return str.All(c => SafeAnsiCharSet.Contains(c));
		}

		public static bool IsValidSafeAnsiString(string str) {
			return SafeAnsiStringRegex.IsMatch(str);
		}
	}
}

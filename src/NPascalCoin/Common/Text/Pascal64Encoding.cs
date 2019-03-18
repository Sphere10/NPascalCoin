using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Sphere10.Framework;

namespace NPascalCoin.Common.Text {
	public class Pascal64Encoding {
		public const char EscapeChar = '\\';
		public const string Pascal64CharSet = "abcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-+{}[]_:`|<>,.?/~";
		public const string Pascal64CharSetStart = "abcdefghijklmnopqrstuvwxyz!@#$%^&*()-+{}[]_:`|<>,.?/~";
		public const string Pascal64CharSetEscaped = "(){}[]:<>";
		public const string Pascal64CharSetUnescaped = "abcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*-+_`|,.?/~";
		public const string Pascal64StartCharPattern = @"(a|b|c|d|e|f|g|h|i|j|k|l|m|n|o|p|q|r|s|t|u|v|w|x|y|z|!|@|#|\$|%|\^|&|\*|\\\(|\\\)|-|\+|\\\{|\\\}|\\\[|\\]|_|\\:|`|\||\\<|\\>|,|\.|\?|/|~)";
		public const string Pascal64NextCharPattern = @"(a|b|c|d|e|f|g|h|i|j|k|l|m|n|o|p|q|r|s|t|u|v|w|x|y|z|0|1|2|3|4|5|6|7|8|9|!|@|#|\$|%|\^|&|\*|\\\(|\\\)|-|\+|\\\{|\\\}|\\\[|\\]|_|\\:|`|\||\\<|\\>|,|\.|\?|/|~)";
		public const string Pascal64StringPattern = Pascal64StartCharPattern + Pascal64NextCharPattern + "{2,63}";
		public const string Pascal64StringOnlyPattern = Pascal64StartCharPattern + Pascal64NextCharPattern + "{2,63}$";
		private static readonly Regex SafePascal64Regex;

		static Pascal64Encoding() {
			SafePascal64Regex = new Regex(Pascal64StringOnlyPattern);
		}

		public static bool IsValid(string unescapedPascal64String) {
			return Pascal64CharSet.All(c => Pascal64CharSet.Contains(c));
		}

		public static bool IsValidUnescaped(string escapedPascal64String) {
			return SafePascal64Regex.IsMatch(escapedPascal64String);
		}

		public static string Escape(string unsafePascal64String) {
			return unsafePascal64String
				.Select(c => Pascal64CharSetEscaped.Contains(c) ? $@"{EscapeChar}{c}" : $"{c}")
				.ToDelimittedString(string.Empty);
		}

		public static string Unescape(string safePascal64String) {
			throw new NotImplementedException();
		}

	}
}
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Sphere10.Framework;

namespace NPascalCoin.Common.Text {
	public class PascalAsciiEncoding {
		public const char EscapeChar = '\\';
		public const string PascalAsciiCharSet = @" !""#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
		public const string PascalAsciiCharSetEscaped = @"""():<>[\]{}";
		public const string PascalAsciiCharSetUnescaped = @" !#$%&'*+,-./0123456789;=?@ABCDEFGHIJKLMNOPQRSTUVWXYZ^_`abcdefghijklmnopqrstuvwxyz|~";
		public const string PascalAsciiCharPattern = @"( |!|\\""|#|\$|%|&|'|\\\(|\\\)|\*|\+|,|-|\.|/|0|1|2|3|4|5|6|7|8|9|\\:|;|\\<|=|\\>|\?|@|A|B|C|D|E|F|G|H|I|J|K|L|M|N|O|P|Q|R|S|T|U|V|W|X|Y|Z|\\\[|\\\\|\\]|\^|_|`|a|b|c|d|e|f|g|h|i|j|k|l|m|n|o|p|q|r|s|t|u|v|w|x|y|z|\\\{|\||\\\}|~)";
		public const string PascalAsciiStringPattern = PascalAsciiCharPattern + "+";

		private static readonly Regex PascalAsciiStringRegex;

		static PascalAsciiEncoding() {
			PascalAsciiStringRegex = new Regex(PascalAsciiStringPattern, RegexOptions.Compiled);
		}

		public static bool IsValidEscaped(string safeAnsiString) {
			return PascalAsciiStringRegex.IsMatch(safeAnsiString);
		}

		public static bool IsValidUnescaped(string unescapedPascalAsciiString) {
			return unescapedPascalAsciiString.All(c => PascalAsciiCharSet.Contains(c));
		}

		public static string Escape(string pascalAsciiString) {
			return pascalAsciiString.Escape(EscapeChar, PascalAsciiCharSetEscaped.ToCharArray());
		}

		public static string Unescape(string pascalAsciiString) {
			return pascalAsciiString.Unescape(EscapeChar, PascalAsciiCharSetEscaped.ToCharArray());
		}

	}
}
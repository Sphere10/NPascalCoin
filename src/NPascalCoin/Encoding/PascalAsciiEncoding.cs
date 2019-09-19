﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Sphere10.Framework;

namespace NPascalCoin.Encoding {
	public static class PascalAsciiEncoding {
		public const char EscapeChar = '\\';
		public const string CharSet = @" !""#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
		public const string CharSetEscaped = @"""():<>[\]{}";
		public const string CharSetUnescaped = @" !#$%&'*+,-./0123456789;=?@ABCDEFGHIJKLMNOPQRSTUVWXYZ^_`abcdefghijklmnopqrstuvwxyz|~";
		public const string CharPattern = @"( |!|\\""|#|\$|%|&|'|\\\(|\\\)|\*|\+|,|-|\.|/|0|1|2|3|4|5|6|7|8|9|\\:|;|\\<|=|\\>|\?|@|A|B|C|D|E|F|G|H|I|J|K|L|M|N|O|P|Q|R|S|T|U|V|W|X|Y|Z|\\\[|\\\\|\\]|\^|_|`|a|b|c|d|e|f|g|h|i|j|k|l|m|n|o|p|q|r|s|t|u|v|w|x|y|z|\\\{|\||\\\}|~)";
		public const string StringPattern = CharPattern + "+";

		private static readonly Regex EscapedStringRegex;

		static PascalAsciiEncoding() {
			EscapedStringRegex = new Regex(StringPattern, RegexOptions.Compiled);
		}

		public static bool IsValidEscaped(string safeAnsiString) {
			return EscapedStringRegex.IsMatch(safeAnsiString);
		}

		public static bool IsValidUnescaped(string unescapedPascalAsciiString) {
			return unescapedPascalAsciiString.All(c => CharSet.Contains(c));
		}

		public static string Escape(string pascalAsciiString) {
			return pascalAsciiString.Escape(EscapeChar, CharSetEscaped.ToCharArray());
		}

		public static string Unescape(string pascalAsciiString) {
			return pascalAsciiString.Unescape(EscapeChar, CharSetEscaped.ToCharArray());
		}

	}
}
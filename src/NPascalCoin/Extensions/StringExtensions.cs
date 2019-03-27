using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using NPascalCoin.Common;
using Sphere10.Framework;

namespace NPascalCoin {
	public static class StringExtensions {
		public static uint ToAccountNumber(this string accountString) {
			return AccountHelper.ParseAccount(accountString);
		}

		public static string Escape(this string str, char escapeSymbol, params char[] escapedChars) {
			if (str == null) 
				throw new ArgumentNullException(nameof(str));
			var result = string.Empty;
			var reader = new StringReader(str);
			char? peek;
			while ((peek = reader.PeekChar()) != null) {
				if (peek == escapeSymbol) {
					result += reader.ReadChar(); // append escape symbol					
					var next = reader.PeekChar();
					if (next == null) {
						// end of string, last char was escape symbol
						if (escapedChars.Contains(escapeSymbol)) {
							// need to escape it 
							result += $"{escapeSymbol}";
						}
					} else if (escapedChars.Contains((char)next)) {
						// is an escape sequence, append next char
						result += reader.ReadChar();
					} else {
						// is an invalid escape sequence
						if (escapedChars.Contains(escapeSymbol)) {
							// need to escape symbol, since it's an escaped char
							result += $"{escapeSymbol}";
						}
					}					
				} else if (escapedChars.Contains((char)peek)) {
					// char needs escaping
					result += $"{escapeSymbol}{reader.ReadChar()}";
				} else {
					// normal char
					result += reader.ReadChar();
				}
			}
			return result;
		}

		public static string Unescape(this string str, char escapeSymbol, params char[] escapedChars) {
			if (str == null)
				throw new ArgumentNullException(nameof(str));
			var result = string.Empty;			
			var reader = new StringReader(str);
			char? peek;
			while ((peek = reader.PeekChar()) != null) {
				if (peek == escapeSymbol) {
					reader.ReadChar(); // omit the escape symbol
					peek = reader.PeekChar();
					if (peek == null) {
						// last character was the escape symbol, so include it
						result += escapeSymbol;
						break;
					}
					if (!escapedChars.Contains((char)peek)) {
						// was not an escaped char, so include the escape symbol
						result += escapeSymbol;
						continue;
					}
				}
				// include the char (or escaped char)
				result += reader.ReadChar();
			}
			return result;
		}
	}
}

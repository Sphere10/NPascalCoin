using System;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using NPascalCoin.Common;
using NPascalCoin.Encoding;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Sphere10.Framework;

namespace NPascalCoin.UnitTests.Text {
	public class PascalAsciiEncodingTests {

		#region PascalAsciiChar 

		[Test]
		public void PascalAsciiCharPattern_Unescaped() {
			foreach (var ch in PascalAsciiEncoding.CharSetUnescaped)
				AssertEx.RegexMatch($"{ch}", PascalAsciiEncoding.CharPattern);
		}

		[Test]
		public void PascalAsciiCharPattern_Escaped() {
			foreach (var ch in PascalAsciiEncoding.CharSetEscaped)
				AssertEx.RegexMatch($@"\{ch}", PascalAsciiEncoding.CharPattern);
		}

		[Test]
		public void PascalAsciiCharPattern_IllegalEscape() {
			foreach (var ch in PascalAsciiEncoding.CharSetUnescaped)
				AssertEx.RegexNotMatch($"{PascalAsciiEncoding.EscapeChar}{ch}", PascalAsciiEncoding.CharPattern);
		}

		[Test]
		public void PascalAsciiCharPattern_MissingEscape() {
			foreach (var ch in PascalAsciiEncoding.CharSetEscaped)
				AssertEx.RegexNotMatch($"{ch}", PascalAsciiEncoding.CharPattern);
		}

		[Test]
		public void PascalAsciiCharPattern_IllegalChar() {
			var illegalCharSet = Enumerable
				.Range(0, 255)
				.Select(x => System.Text.Encoding.ASCII.GetString(new[] { (byte)x })[0])
				.Where(c => !PascalAsciiEncoding.CharSet.Contains(c));

			foreach (var ch in illegalCharSet)
				AssertEx.RegexNotMatch($"{ch}", PascalAsciiEncoding.CharPattern);
		}

		#endregion

		#region PascalAsciiString

		[Test]
		public void PascalAsciiStringPattern_Unescaped() {
			AssertEx.RegexMatch(PascalAsciiEncoding.CharSetUnescaped, PascalAsciiEncoding.StringPattern);
		}

		[Test]
		public void PascalAsciiStringPattern_Escaped() {
			var input = @"proper\\escape";
			AssertEx.RegexMatch(input, PascalAsciiEncoding.StringPattern);
		}

		[Test]
		public void PascalAsciiStringPattern_FullCharSet_Escaped() {
			AssertEx.RegexMatch(PascalAsciiEncoding.Escape(PascalAsciiEncoding.CharSet), PascalAsciiEncoding.StringPattern);
		}

		[Test]
		public void PascalAsciiStringPattern_IllegalEscape() {
			var badInput = @"illegal\escape";
			AssertEx.RegexNotMatch(badInput, PascalAsciiEncoding.StringPattern);
		}

		[Test]
		public void PascalAsciiStringPattern_MissingEscape() {
			var badInput = @"missing[]escape";
			AssertEx.RegexNotMatch(badInput, PascalAsciiEncoding.StringPattern);
		}

		[Test]
		public void PascalAsciiStringPattern_IllegalCharSet() {
			var illegalCharSet =
				Enumerable
					.Range(0, 255)
					.Select(x => System.Text.Encoding.ASCII.GetString(new[] { (byte)x })[0])
					.Where(c => !PascalAsciiEncoding.CharSet.Contains(c))
					.ToDelimittedString(string.Empty);

			AssertEx.RegexNotMatch(illegalCharSet, PascalAsciiEncoding.StringPattern);
			AssertEx.RegexNotMatch(PascalAsciiEncoding.CharSetUnescaped + illegalCharSet, PascalAsciiEncoding.StringPattern);
			AssertEx.RegexNotMatch(illegalCharSet + PascalAsciiEncoding.CharSetUnescaped, PascalAsciiEncoding.StringPattern);
		}

		#endregion

		#region Misc

		[Test]
		public void Encoding_EscapeString() {
			var unescaped = @"""a(b)c:d<e>f[g\h]i{j}";
			var escaped = @"\""a\(b\)c\:d\<e\>f\[g\\h\]i\{j\}";
			Assert.AreEqual(escaped, PascalAsciiEncoding.Escape(unescaped));
			Assert.AreEqual(escaped, PascalAsciiEncoding.Escape(PascalAsciiEncoding.Escape(unescaped)));
			Assert.IsTrue(PascalAsciiEncoding.IsValidEscaped(escaped));
			Assert.IsTrue(PascalAsciiEncoding.IsValidEscaped(unescaped)); // unescaped is also a valid escaped string (unlike pascal64 encoding)
		}

		[Test]
		public void Encoding_UnescapedString() {
			var unescaped = @"""a(b)c:d<e>f[g\h]i{j}";
			var escaped = @"\""a\(b\)c\:d\<e\>f\[g\\h\]i\{j\}";
			Assert.AreEqual(unescaped, PascalAsciiEncoding.Unescape(escaped));
			Assert.AreEqual(unescaped, PascalAsciiEncoding.Unescape(PascalAsciiEncoding.Unescape(escaped)));
			Assert.IsTrue(PascalAsciiEncoding.IsValidUnescaped(escaped)); // escaped string is also valid as unescaped, since \ is allowed
			Assert.IsTrue(PascalAsciiEncoding.IsValidUnescaped(unescaped));
		}

		#endregion

	}
}
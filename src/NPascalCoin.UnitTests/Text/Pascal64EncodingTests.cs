using System;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using NPascalCoin.Common;
using NPascalCoin.Common.Text;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Sphere10.Framework;

namespace NPascalCoin.UnitTests.Text {
	public class Pascal64EncodingTests {

		#region Pascal64Char

		[Test]
		public void Pascal64StartChar_Unescaped() {
			foreach (var ch in Pascal64Encoding.Pascal64CharSetStart.Where(c => !Pascal64Encoding.Pascal64CharSetEscaped.Contains(c)))
				AssertEx.RegexMatch($"{ch}", Pascal64Encoding.Pascal64StartCharPattern);
		}

		[Test]
		public void Pascal64StartChar_Escaped() {
			foreach (var ch in Pascal64Encoding.Pascal64CharSetStart.Where(c => Pascal64Encoding.Pascal64CharSetEscaped.Contains(c)))
				AssertEx.RegexMatch($@"\{ch}", Pascal64Encoding.Pascal64StartCharPattern);
		}

		[Test]
		public void Pascal64StartChar_IllegalEscape() {
			foreach (var ch in Pascal64Encoding.Pascal64CharSetStart.Where(c => !Pascal64Encoding.Pascal64CharSetEscaped.Contains(c)))
				AssertEx.RegexNotMatch($@"\{ch}", Pascal64Encoding.Pascal64StartCharPattern);
		}

		[Test]
		public void Pascal64StartChar_MissingEscape() {
			foreach (var ch in Pascal64Encoding.Pascal64CharSetStart.Where(c => Pascal64Encoding.Pascal64CharSetEscaped.Contains(c)))
				AssertEx.RegexNotMatch($"{ch}", Pascal64Encoding.Pascal64StartCharPattern);
		}

		[Test]
		public void Pascal64StartChar_IllegalChar() {
			var illegalCharSet = Enumerable
				.Range(0, 255)
				.Select(x => Encoding.ASCII.GetString(new[] { (byte)x })[0])
				.Where(c => !Pascal64Encoding.Pascal64CharSetStart.Contains(c));

			foreach (var ch in illegalCharSet)
				AssertEx.RegexNotMatch($"{ch}", Pascal64Encoding.Pascal64StartCharPattern);
		}

		[Test]
		public void Pascal64NextChar_Unescaped() {
			foreach (var ch in Pascal64Encoding.Pascal64CharSetUnescaped)
				AssertEx.RegexMatch($"{ch}", Pascal64Encoding.Pascal64NextCharPattern);
		}

		[Test]
		public void Pascal64NextChar_Escaped() {
			foreach (var ch in Pascal64Encoding.Pascal64CharSetEscaped)
				AssertEx.RegexMatch($@"\{ch}", Pascal64Encoding.Pascal64NextCharPattern);
		}

		[Test]
		public void Pascal64NextChar_IllegalEscape() {
			foreach (var ch in Pascal64Encoding.Pascal64CharSetUnescaped)
				AssertEx.RegexNotMatch($@"\{ch}", Pascal64Encoding.Pascal64NextCharPattern);
		}

		[Test]
		public void Pascal64NextChar_MissingEscape() {
			foreach (var ch in Pascal64Encoding.Pascal64CharSetEscaped)
				AssertEx.RegexNotMatch($"{ch}", Pascal64Encoding.Pascal64NextCharPattern);
		}

		[Test]
		public void Pascal64NextChar_IllegalChar() {
			var illegalCharSet = Enumerable
				.Range(0, 255)
				.Select(x => Encoding.ASCII.GetString(new[] { (byte)x })[0])
				.Where(c => !Pascal64Encoding.Pascal64CharSet.Contains(c));

			foreach (var ch in illegalCharSet)
				AssertEx.RegexNotMatch($"{ch}", Pascal64Encoding.Pascal64NextCharPattern);
		}

		#endregion

		#region Pascal64Strings

		[Test]
		public void Pascal64StringPattern_Unescaped() {
			AssertEx.RegexMatch(Pascal64Encoding.Pascal64CharSetUnescaped, Pascal64Encoding.Pascal64StringPattern);
		}

		[Test]
		public void Pascal64StringPattern_Escaped() {
			var badInput = @"correctly\[\]escaped";
			AssertEx.RegexMatch(badInput, Pascal64Encoding.Pascal64StringPattern);
		}

		[Test]
		public void Pascal64StringPattern_FullCharSet_Escaped() {
			AssertEx.RegexMatch(Pascal64Encoding.Escape(Pascal64Encoding.Pascal64CharSet), PascalAsciiEncoding.PascalAsciiStringPattern);
		}

		[Test]
		public void Pascal64StringPattern_IllegalEscape() {
			var badInput = @"illegal\escape";
			AssertEx.RegexNotMatch(badInput, Pascal64Encoding.Pascal64StringPattern);
		}

		[Test]
		public void Pascal64StringPattern_MissingEscape() {
			var badInput = @"missing[]escape";
			AssertEx.RegexNotMatch(badInput, Pascal64Encoding.Pascal64StringPattern);
		}

		[Test]
		public void Pascal64StringPattern_IllegalCharSet() {
			var illegalCharSet =
				Enumerable
					.Range(0, 255)
					.Select(x => Encoding.ASCII.GetString(new[] { (byte)x })[0])
					.Where(c => !Pascal64Encoding.Pascal64CharSet.Contains(c))
					.ToDelimittedString(string.Empty);

			foreach (var subset in illegalCharSet.Partition(63)) {
				var testSet = "b" + new string(subset.ToArray());
				AssertEx.RegexNotMatch(testSet, Pascal64Encoding.Pascal64StringPattern);
			}
		}

		[Test]
		public void Pascal64StringPattern_TooShort() {
			var badInput = @"bb";
			AssertEx.RegexNotMatch(badInput, Pascal64Encoding.Pascal64StringPattern);
		}

		[Test]
		public void Pascal64StringPattern_TooLong() {
			var badInput = Enumerable.Range(0, 65).Select(c => 'b').ToDelimittedString(string.Empty);
			AssertEx.RegexNotMatch(badInput, Pascal64Encoding.Pascal64StringPattern);
		}


		#endregion

		#region Misc

		[Test]
		public void Encoding_EscapeString() {
			var escaped = @"\(a\)b\{c\}d\[e\]f\:g\""h\<i\>";
			var unescaped = @"(a)b{c}d[e]f:g""h<i>";
			Assert.AreEqual(escaped, Pascal64Encoding.Escape(unescaped));
			Assert.AreEqual(escaped, Pascal64Encoding.Escape(Pascal64Encoding.Escape(unescaped)));
			Assert.IsFalse(Pascal64Encoding.IsValidEscaped(unescaped));
			Assert.IsTrue(Pascal64Encoding.IsValidEscaped(escaped));			
		}

		[Test]
		public void Encoding_UnescapedString() {
			var escaped = @"\(a\)b\{c\}d\[e\]f\:g\""h\<i\>";
			var unescaped = @"(a)b{c}d[e]f:g""h<i>";
			Assert.AreEqual(unescaped, Pascal64Encoding.Unescape(escaped));
			Assert.AreEqual(unescaped, Pascal64Encoding.Unescape(Pascal64Encoding.Unescape(escaped)));
			Assert.IsFalse(Pascal64Encoding.IsValidUnescaped(escaped)); 
			Assert.IsTrue(Pascal64Encoding.IsValidUnescaped(unescaped));
		}

		#endregion
	}
}
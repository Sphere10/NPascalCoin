using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NPascalCoin.Common.Text;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Sphere10.Framework;

namespace NPascalCoin.UnitTests {

	public static class AssertEx {
		

		public static void RegexMatch(string expected, string regexPattern, params Tuple<string, string>[] expectedCaptures) {
			var regex = new Regex(regexPattern);
			var match = regex.Match(expected);
			Assert.AreEqual(expected, match.Value);
			foreach (var expectedCapture in expectedCaptures) {
				if (expectedCapture.Item2 == null)
					Assert.IsFalse(match.Groups[expectedCapture.Item1].Success);
				else
					Assert.AreEqual(expectedCapture.Item2, match.Groups[expectedCapture.Item1]?.Value);
			}
		}

		public static void RegexNotMatch(string badInput, string regexPattern, params Tuple<string, string>[] expectedCaptures) {
			var regex = new Regex(regexPattern);
			var match = regex.Match(badInput);
			Assert.AreNotEqual(badInput, match.Value);
			if (expectedCaptures.Any())
			  Assert.IsFalse(expectedCaptures.All(c => c.Item2 == match.Groups[c.Item1]?.Value));

		}
	}
}
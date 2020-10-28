using System;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using NPascalCoin.Encoding;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Sphere10.Framework;

namespace NPascalCoin.UnitTests.Text {
	public class PascalBase58EncodingTests {
		[SetUp]
		public void Setup() {
		}

		[Test]
		public void Compatibility_Test() {
			// Test values derived from Pascal implementation
			const string keyBase58 = "3GhhbonKEE3SzPvRqPEeXbLc1v1LCcYvqY7wuxW8esJLCb2FCj4jofWmkmMTtqP1atrmioAMqEwigHA2CEwidxgB1i1gGhL39unBkk";
			const string keyHex = "ca0220003ce142a2bfa1e2d4c6246bf7dfa8106d975f498133fb7084e83062f8941d09ea2000ab8fa84d3287d8efe04b865e50f9c7ff2a9a85187191c2528045e697ec9d8b5143db2899";
			Assert.AreEqual(keyBase58, PascalBase58Encoding.Encode(keyHex.ToHexByteArray()));
			Assert.AreEqual(keyHex, PascalBase58Encoding.Decode(keyBase58).ToHexString(true));			
		}

	}
}
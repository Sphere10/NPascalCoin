using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NPascalCoin.Common;
using NPascalCoin.Common.Text;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Sphere10.Framework;

namespace NPascalCoin.UnitTests.Text {
	public abstract class IEPasaParserTests {
		[SetUp]
		public void Setup() {
		}

		public abstract IEPasaParser NewInstance();

		[Test]
		public void AccountNumber() {
			var parser = NewInstance();
			Assert.IsTrue(parser.TryParse("77", out var epasa));
			Assert.AreEqual(77, epasa.Account);
			Assert.AreEqual(null, epasa.AccountChecksum);
			Assert.AreEqual(null, epasa.ExtendedChecksum);
			Assert.AreEqual(PayloadType.NonDeterministic, epasa.PayloadType);
			Assert.AreEqual("77", epasa.ToString());
		}

		[Test]
		public void AccountNumber_Illegal() {
			var parser = NewInstance();
			Assert.IsFalse(parser.TryParse("077", out var epasa));
			Assert.IsFalse(parser.TryParse("77s-44", out epasa));
		}

		[Test]
		public void AccountNumber_Checksum() {
			var parser = NewInstance();
			Assert.IsTrue(parser.TryParse("77-44", out var epasa));
			Assert.AreEqual(77, epasa.Account);
			Assert.AreEqual(44, epasa.AccountChecksum);
			Assert.AreEqual(null, epasa.ExtendedChecksum);
			Assert.AreEqual(PayloadType.NonDeterministic, epasa.PayloadType);
			Assert.AreEqual("77-44", epasa.ToString());
		}

		[Test]
		public void AccountNumber_Checksum_Illegal() {
			var parser = NewInstance();
			Assert.IsFalse(parser.TryParse("77- 44", out var epasa));
			Assert.IsFalse(parser.TryParse("77-444", out epasa));
			Assert.IsFalse(parser.TryParse("77-4c", out epasa));
		}



		[Test]
		public void AccountNumber_ExtendedChecksum() {
			var parser = NewInstance();
			Assert.IsTrue(parser.TryParse("77-44:12ab", out var epasa));
			Assert.AreEqual(77, epasa.Account);
			Assert.AreEqual(44, epasa.AccountChecksum);
			Assert.AreEqual(null, epasa.AccountChecksum);
			Assert.AreEqual("12ab", epasa.ExtendedChecksum);
			Assert.AreEqual(PayloadType.NonDeterministic, epasa.PayloadType);
			Assert.AreEqual("77-44:12ab", epasa.ToString());
		}

		[Test]
		public void AccountNumber_ExtendedChecksum_Illegal() {
		}


		[Test]
		public void AccountNumber_Payload_ExtendedChecksum() {
			var parser = NewInstance();
			Assert.IsTrue(parser.TryParse(@"77-44[""Hello World!""]:12ab", out var epasa));
			Assert.AreEqual(77, epasa.Account);
			Assert.AreEqual(44, epasa.AccountChecksum);
			Assert.AreEqual("12ab", epasa.ExtendedChecksum);
			Assert.AreEqual("Hello World!", epasa.Payload);
			Assert.AreEqual(PayloadType.Public | PayloadType.AsciiFormatted, epasa.PayloadType);
			Assert.AreEqual(@"77-44[""Hello World!""]:12ab", epasa.ToString());
		}


		[Test]
		public void AccountNumberWithChecksum() {
			var parser = NewInstance();
			Assert.IsTrue(parser.TryParse("77-44", out var epasa));
			Assert.AreEqual(77, epasa.Account);
			Assert.AreEqual(44, epasa.AccountChecksum);
			Assert.AreEqual("77-44", epasa.ToString());
		}

	}
}
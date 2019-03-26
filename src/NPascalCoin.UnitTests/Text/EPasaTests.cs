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
	public abstract class EPasaTests {
		[SetUp]
		public void Setup() {
		}

		public abstract IEPasaParser NewInstance();

		[Test]
		public void AccountNumber() {
			var parser = NewInstance();
			Assert.IsTrue(parser.TryParse("77", out var epasa));
			Assert.AreEqual(77, epasa.Account);
			Assert.AreEqual(44, epasa.AccountChecksum);
			Assert.AreEqual(EPasaHelper.ComputeExtendedChecksum("77-44"), epasa.ExtendedChecksum);
			Assert.AreEqual(PayloadType.NonDeterministic, epasa.PayloadType);
			Assert.AreEqual($"77-44:{EPasaHelper.ComputeExtendedChecksum("77-44")}", epasa.ToString());
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
			Assert.AreEqual(EPasaHelper.ComputeExtendedChecksum("77-44"), epasa.ExtendedChecksum);
			Assert.AreEqual(PayloadType.NonDeterministic, epasa.PayloadType);
			Assert.AreEqual($"77-44:{EPasaHelper.ComputeExtendedChecksum("77-44")}", epasa.ToString());
		}

		[Test]
		public void AccountNumber_0_10() {
			var parser = NewInstance();
			Assert.IsTrue(parser.TryParse("0-10", out var epasa));
			Assert.AreEqual(0, epasa.Account);
			Assert.AreEqual(10, epasa.AccountChecksum);
			Assert.AreEqual(EPasaHelper.ComputeExtendedChecksum("0-10"), epasa.ExtendedChecksum);
			Assert.AreEqual(PayloadType.NonDeterministic, epasa.PayloadType);
			Assert.AreEqual($"0-10:{EPasaHelper.ComputeExtendedChecksum("0-10")}", epasa.ToString());
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
			var epasaText = "77-44";
			var checksum = EPasaHelper.ComputeExtendedChecksum(epasaText);
			epasaText = $"{epasaText}:{checksum}";
			Assert.IsTrue(parser.TryParse(epasaText, out var epasa));
			Assert.AreEqual(77, epasa.Account);
			Assert.AreEqual(44, epasa.AccountChecksum);
			Assert.AreEqual(checksum, epasa.ExtendedChecksum);
			Assert.AreEqual(PayloadType.NonDeterministic, epasa.PayloadType);
			Assert.AreEqual(epasaText, epasa.ToString());
		}

		[Test]
		public void AccountNumber_ExtendedChecksum_Illegal() {
			var parser = NewInstance();
			var epasaText = "77-44:0000";
			Assert.IsFalse(parser.TryParse(epasaText, out var epasa));
		}

		[Test]
		public void AccountNumber_Payload_ExtendedChecksum() {
			var parser = NewInstance();
			var epasaText = @"77-44[""Hello World!""]";
			var checksum = EPasaHelper.ComputeExtendedChecksum(epasaText);
			epasaText = $"{epasaText}:{checksum}";
			Assert.IsTrue(parser.TryParse(epasaText, out var epasa));
			Assert.AreEqual(77, epasa.Account);
			Assert.AreEqual(44, epasa.AccountChecksum);
			Assert.AreEqual(checksum, epasa.ExtendedChecksum);
			Assert.AreEqual(PayloadType.Public | PayloadType.AsciiFormatted, epasa.PayloadType);
			Assert.AreEqual("Hello World!", epasa.Payload);
			Assert.AreEqual(epasaText, epasa.ToString());
		}

		[Test]
		public void AccountName() {
			var parser = NewInstance();
			var epasaText = "pascalcoin-foundation";
			var checksum = EPasaHelper.ComputeExtendedChecksum(epasaText);
			Assert.IsTrue(parser.TryParse(epasaText, out var epasa));
			Assert.AreEqual(null, epasa.Account);
			Assert.AreEqual(null, epasa.AccountChecksum);
			Assert.AreEqual("pascalcoin-foundation", epasa.AccountName);
			Assert.AreEqual(checksum, epasa.ExtendedChecksum);
			Assert.AreEqual(PayloadType.NonDeterministic | PayloadType.AddressedByName, epasa.PayloadType);
			Assert.AreEqual($"{epasaText}:{checksum}", epasa.ToString());
		}

		[Test]
		public void AccountName_ExtendedChecksum() {
			var parser = NewInstance();
			var epasaText = "pascalcoin-foundation";
			var checksum = EPasaHelper.ComputeExtendedChecksum(epasaText);
			epasaText = $"{epasaText}:{checksum}";
			Assert.IsTrue(parser.TryParse(epasaText, out var epasa));
			Assert.AreEqual(null, epasa.Account);
			Assert.AreEqual(null, epasa.AccountChecksum);
			Assert.AreEqual("pascalcoin-foundation", epasa.AccountName);
			Assert.AreEqual(checksum, epasa.ExtendedChecksum);
			Assert.AreEqual(PayloadType.NonDeterministic | PayloadType.AddressedByName, epasa.PayloadType);
			Assert.AreEqual(epasaText, epasa.ToString());
		}

		[Test]
		public void AccountName_Payload_ExtendedChecksum() {
			var parser = NewInstance();
			var epasaText = @"pascalcoin-foundation[""Hello World!""]";
			var checksum = EPasaHelper.ComputeExtendedChecksum(epasaText);
			epasaText = $"{epasaText}:{checksum}";
			Assert.IsTrue(parser.TryParse(epasaText, out var epasa));
			Assert.AreEqual(null, epasa.Account);
			Assert.AreEqual(null, epasa.AccountChecksum);
			Assert.AreEqual("pascalcoin-foundation", epasa.AccountName);
			Assert.AreEqual(checksum, epasa.ExtendedChecksum);
			Assert.AreEqual(PayloadType.Public | PayloadType.AsciiFormatted | PayloadType.AddressedByName, epasa.PayloadType);
			Assert.AreEqual("Hello World!", epasa.Payload);
			Assert.AreEqual(epasaText, epasa.ToString());
		}

	}
}
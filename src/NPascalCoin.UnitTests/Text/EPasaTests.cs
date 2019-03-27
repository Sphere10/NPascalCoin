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
		public void AccountNumber_Password_Valid() {
			var parser = NewInstance();
			var epasaText = @"77-44{""Hello World!"":abcdefg}";
			var checksum = EPasaHelper.ComputeExtendedChecksum(epasaText);
			epasaText = $"{epasaText}:{checksum}";
			Assert.IsTrue(parser.TryParse(epasaText, out var epasa));
			Assert.AreEqual(77, epasa.Account);
			Assert.AreEqual(44, epasa.AccountChecksum);
			Assert.AreEqual(checksum, epasa.ExtendedChecksum);
			Assert.AreEqual("abcdefg", epasa.Password);
			Assert.AreEqual(PayloadType.PasswordEncrypted | PayloadType.AsciiFormatted, epasa.PayloadType);
			Assert.AreEqual("Hello World!", epasa.Payload);
			Assert.AreEqual(epasaText, epasa.ToString());
		}

		[Test]
		public void AccountNumber_EncryptionAndEncoding() {
			var parser = NewInstance();

			Assert.AreEqual(PayloadType.Public | PayloadType.AsciiFormatted, parser.Parse(@"77-44[""Hello World!""]").PayloadType);
			Assert.AreEqual(PayloadType.Public | PayloadType.HexFormatted, parser.Parse(@"77-44[0x1234]").PayloadType);
			Assert.AreEqual(PayloadType.Public | PayloadType.Base58Formatted, parser.Parse(@"77-44[B58abcdefg]").PayloadType);

			Assert.AreEqual(PayloadType.RecipientKeyEncrypted | PayloadType.AsciiFormatted, parser.Parse(@"77-44(""Hello World!"")").PayloadType);
			Assert.AreEqual(PayloadType.RecipientKeyEncrypted | PayloadType.HexFormatted, parser.Parse(@"77-44(0x1234)").PayloadType);
			Assert.AreEqual(PayloadType.RecipientKeyEncrypted | PayloadType.Base58Formatted, parser.Parse(@"77-44(B58abcdefg)").PayloadType);

			Assert.AreEqual(PayloadType.SenderKeyEncrypted | PayloadType.AsciiFormatted, parser.Parse(@"77-44<""Hello World!"">").PayloadType);
			Assert.AreEqual(PayloadType.SenderKeyEncrypted | PayloadType.HexFormatted, parser.Parse(@"77-44<0x1234>").PayloadType);
			Assert.AreEqual(PayloadType.SenderKeyEncrypted | PayloadType.Base58Formatted, parser.Parse(@"77-44<B58abcdefg>").PayloadType);

			Assert.AreEqual(PayloadType.PasswordEncrypted | PayloadType.AsciiFormatted, parser.Parse(@"77-44{""Hello World!"":abc}").PayloadType);
			Assert.AreEqual(PayloadType.PasswordEncrypted | PayloadType.HexFormatted, parser.Parse(@"77-44{0x1234:abc}").PayloadType);
			Assert.AreEqual(PayloadType.PasswordEncrypted | PayloadType.Base58Formatted, parser.Parse(@"77-44{B58abcdefg:abc}").PayloadType);

		}

		[Test]
		public void AccountNumber_NonDeterministic_PayloadType() {
			var parser = NewInstance();
			Assert.AreEqual(PayloadType.NonDeterministic, parser.Parse(@"77-44").PayloadType);
			Assert.AreEqual(PayloadType.Public | PayloadType.NonDeterministic, parser.Parse(@"77-44[]").PayloadType);
			Assert.AreEqual(PayloadType.RecipientKeyEncrypted | PayloadType.NonDeterministic, parser.Parse(@"77-44()").PayloadType);
			Assert.AreEqual(PayloadType.SenderKeyEncrypted | PayloadType.NonDeterministic, parser.Parse(@"77-44<>").PayloadType);
			Assert.AreEqual(PayloadType.PasswordEncrypted | PayloadType.NonDeterministic, parser.Parse(@"77-44{:abc}").PayloadType);
			Assert.AreEqual(PayloadType.PasswordEncrypted | PayloadType.NonDeterministic, parser.Parse(@"77-44{:}").PayloadType);
		}

		[Test]
		public void AccountNumber_NonDeterministic_ToString() {
			var parser = NewInstance();
			Assert.AreEqual("77-44", parser.Parse(@"77-44").ToString(true));
			Assert.AreEqual("77-44[]", parser.Parse("77-44[]").ToString(true));
			Assert.AreEqual("77-44()", parser.Parse("77-44()").ToString(true));
			Assert.AreEqual("77-44<>", parser.Parse("77-44<>").ToString(true));
			Assert.AreEqual("77-44{:abc}", parser.Parse("77-44{:abc}").ToString(true));
			Assert.AreEqual("77-44{:}", parser.Parse("77-44{:}").ToString(true));
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

		[Test]
		public void AccountName_EncryptionAndEncoding() {
			var parser = NewInstance();

			Assert.AreEqual(PayloadType.AddressedByName | PayloadType.Public | PayloadType.AsciiFormatted, parser.Parse(@"pascalcoin-foundation[""Hello World!""]").PayloadType);
			Assert.AreEqual(PayloadType.AddressedByName | PayloadType.Public | PayloadType.HexFormatted, parser.Parse(@"pascalcoin-foundation[0x1234]").PayloadType);
			Assert.AreEqual(PayloadType.AddressedByName | PayloadType.Public | PayloadType.Base58Formatted, parser.Parse(@"pascalcoin-foundation[B58abcdefg]").PayloadType);

			Assert.AreEqual(PayloadType.AddressedByName | PayloadType.RecipientKeyEncrypted | PayloadType.AsciiFormatted, parser.Parse(@"pascalcoin-foundation(""Hello World!"")").PayloadType);
			Assert.AreEqual(PayloadType.AddressedByName | PayloadType.RecipientKeyEncrypted | PayloadType.HexFormatted, parser.Parse(@"pascalcoin-foundation(0x1234)").PayloadType);
			Assert.AreEqual(PayloadType.AddressedByName | PayloadType.RecipientKeyEncrypted | PayloadType.Base58Formatted, parser.Parse(@"pascalcoin-foundation(B58abcdefg)").PayloadType);

			Assert.AreEqual(PayloadType.AddressedByName | PayloadType.SenderKeyEncrypted | PayloadType.AsciiFormatted, parser.Parse(@"pascalcoin-foundation<""Hello World!"">").PayloadType);
			Assert.AreEqual(PayloadType.AddressedByName | PayloadType.SenderKeyEncrypted | PayloadType.HexFormatted, parser.Parse(@"pascalcoin-foundation<0x1234>").PayloadType);
			Assert.AreEqual(PayloadType.AddressedByName | PayloadType.SenderKeyEncrypted | PayloadType.Base58Formatted, parser.Parse(@"pascalcoin-foundation<B58abcdefg>").PayloadType);

			Assert.AreEqual(PayloadType.AddressedByName | PayloadType.PasswordEncrypted | PayloadType.AsciiFormatted, parser.Parse(@"pascalcoin-foundation{""Hello World!"":abc}").PayloadType);
			Assert.AreEqual(PayloadType.AddressedByName | PayloadType.PasswordEncrypted | PayloadType.HexFormatted, parser.Parse(@"pascalcoin-foundation{0x1234:abc}").PayloadType);
			Assert.AreEqual(PayloadType.AddressedByName | PayloadType.PasswordEncrypted | PayloadType.Base58Formatted, parser.Parse(@"pascalcoin-foundation{B58abcdefg:abc}").PayloadType);
		}

		[Test]
		public void AccountName_NonDeterministic_PayloadType() {
			var parser = NewInstance();
			Assert.AreEqual(PayloadType.AddressedByName | PayloadType.NonDeterministic, parser.Parse(@"pascalcoin-foundation").PayloadType);
			Assert.AreEqual(PayloadType.AddressedByName | PayloadType.Public | PayloadType.NonDeterministic, parser.Parse(@"pascalcoin-foundation[]").PayloadType);
			Assert.AreEqual(PayloadType.AddressedByName | PayloadType.RecipientKeyEncrypted | PayloadType.NonDeterministic, parser.Parse(@"pascalcoin-foundation()").PayloadType);
			Assert.AreEqual(PayloadType.AddressedByName | PayloadType.SenderKeyEncrypted | PayloadType.NonDeterministic, parser.Parse(@"pascalcoin-foundation<>").PayloadType);
			Assert.AreEqual(PayloadType.AddressedByName | PayloadType.PasswordEncrypted | PayloadType.NonDeterministic, parser.Parse(@"pascalcoin-foundation{:abc}").PayloadType);
			Assert.AreEqual(PayloadType.AddressedByName | PayloadType.PasswordEncrypted | PayloadType.NonDeterministic, parser.Parse(@"pascalcoin-foundation{:}").PayloadType);
		}

		[Test]
		public void AccountName_NonDeterministic_ToString() {
			var parser = NewInstance();
			Assert.AreEqual("pascalcoin-foundation", parser.Parse(@"pascalcoin-foundation").ToString(true));
			Assert.AreEqual("pascalcoin-foundation[]", parser.Parse("pascalcoin-foundation[]").ToString(true));
			Assert.AreEqual("pascalcoin-foundation()", parser.Parse("pascalcoin-foundation()").ToString(true));
			Assert.AreEqual("pascalcoin-foundation<>", parser.Parse("pascalcoin-foundation<>").ToString(true));
			Assert.AreEqual("pascalcoin-foundation{:abc}", parser.Parse("pascalcoin-foundation{:abc}").ToString(true));
			Assert.AreEqual("pascalcoin-foundation{:}", parser.Parse("pascalcoin-foundation{:}").ToString(true));
		}

		[Test]
		public void EdgeCase_AllEscapeChars() {
			var parser = NewInstance();
			var name = @"(a)b{c}d[e]f:g""h<i>";
			var content = @"""a(b)c:d<e>f[g\h]i{j}";
			var password = name;
			var epasaText = $@"{Pascal64Encoding.Escape(name)}{{""{PascalAsciiEncoding.Escape(content)}"":{PascalAsciiEncoding.Escape(password)}}}";
			var checksum = EPasaHelper.ComputeExtendedChecksum(epasaText);
			epasaText = $"{epasaText}:{checksum}";
			Assert.IsTrue(parser.TryParse(epasaText, out var epasa));
			Assert.AreEqual(null, epasa.Account);
			Assert.AreEqual(null, epasa.AccountChecksum);
			Assert.AreEqual(name, epasa.AccountName);
			Assert.AreEqual(checksum, epasa.ExtendedChecksum);
			Assert.AreEqual(password, epasa.Password);
			Assert.AreEqual(PayloadType.AddressedByName | PayloadType.PasswordEncrypted | PayloadType.AsciiFormatted, epasa.PayloadType);
			Assert.AreEqual(content, epasa.Payload);
			Assert.AreEqual(epasaText, epasa.ToString());		
		}

	}
}
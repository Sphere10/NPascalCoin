using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPascalCoin.Common.Text;
using NUnit.Framework;
using NPascalCoin.UnitTests;

namespace NPascalCoin.UnitTests.Text {
	public class RegexEPasaParserTest : EPasaTests {
		public override IEPasaParser NewInstance() {
			return new RegexEPasaParser();
		}


		#region Integer

		[Test]
		public void IntegerPattern_Standard() {
			AssertEx.RegexMatch("1234567890", RegexEPasaParser.IntegerPattern);
		}

		[Test]
		public void IntegerPattern_Starts0() {
			AssertEx.RegexNotMatch("01234567890", RegexEPasaParser.IntegerPattern);
		}

		[Test]
		public void IntegerPattern_Space() {
			AssertEx.RegexNotMatch("12 34567890", RegexEPasaParser.IntegerPattern);
		}

		#endregion

		#region Hex

		[Test]
		public void HexNibble_Valid() {
			foreach (var ch in HexEncoding.HexStringCharSet)
				AssertEx.RegexMatch($"{ch}", HexEncoding.HexNibblePattern);
		}

		[Test]
		public void HexNibble_Illegal() {
			var illegalCharSet = Enumerable
				.Range(0, 255)
				.Select(x => Encoding.ASCII.GetString(new[] { (byte)x })[0])
				.Where(c => !HexEncoding.HexStringCharSet.Contains(c));

			foreach (var ch in illegalCharSet)
				AssertEx.RegexNotMatch($"{ch}", HexEncoding.HexNibblePattern);
		}

		[Test]
		public void HexByte_Valid() {
			foreach (var n1 in HexEncoding.HexStringCharSet)
				foreach (var n2 in HexEncoding.HexStringCharSet)
					AssertEx.RegexMatch($"{n1}{n2}", HexEncoding.HexBytePattern);
		}

		[Test]
		public void HexByte_TooShort() {
			AssertEx.RegexNotMatch("f", HexEncoding.HexBytePattern);
		}

		[Test]
		public void HexByte_TooLong() {
			AssertEx.RegexNotMatch("abf", HexEncoding.HexBytePattern);
		}

		[Test]
		public void HexString_FullCharSet() {
			AssertEx.RegexMatch(HexEncoding.HexStringCharSet, HexEncoding.HexStringPattern);
		}

		#endregion

		#region Base58

		[Test]
		public void Base58Char_Valid() {
			foreach (var ch in PascalBase58Encoding.CharSet)
				AssertEx.RegexMatch($"{ch}", PascalBase58Encoding.CharPattern);
		}

		[Test]
		public void Base58Char_Illegal() {
			var illegalCharSet = Enumerable
				.Range(0, 255)
				.Select(x => Encoding.ASCII.GetString(new[] { (byte)x })[0])
				.Where(c => !PascalBase58Encoding.CharSet.Contains(c));

			foreach (var ch in illegalCharSet)
				AssertEx.RegexNotMatch($"{ch}", PascalBase58Encoding.CharPattern);
		}

		[Test]
		public void Base58String_FullCharSet() {
			AssertEx.RegexMatch(PascalBase58Encoding.CharSet, PascalBase58Encoding.SubStringPattern);
		}

		#endregion

		#region AccountNumber

		[Test]
		public void AccountNumber_Full() {
			AssertEx.RegexMatch("77-44", RegexEPasaParser.AccountNumberPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("Checksum", "44"), Tuple.Create("ChecksumDelim", "-"));
		}

		[Test]
		public void AccountNumber_Whitespace() {
			AssertEx.RegexNotMatch("77- 44", RegexEPasaParser.AccountNumberPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("Checksum", "44"), Tuple.Create("ChecksumDelim", "-"));
		}

		[Test]
		public void AccountNumber_NoChecksum() {
			AssertEx.RegexMatch("77", RegexEPasaParser.AccountNumberPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("Checksum", null as string), Tuple.Create("ChecksumDelim", null as string));
		}

		[Test]
		public void AccountNumber_MalformedChecksum() {
			// not expecting delim
			AssertEx.RegexNotMatch("77-", RegexEPasaParser.AccountNumberPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("Checksum", null as string), Tuple.Create("ChecksumDelim", "-"));
		}

		#endregion

		#region PASA 

		[Test]
		public void PASA_AccountNumber() {
			AssertEx.RegexMatch("77-44", RegexEPasaParser.PasaPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("Checksum", "44"), Tuple.Create("ChecksumDelim", "-"));
		}

		[Test]
		public void PASA_AccountName() {
			AssertEx.RegexMatch("my-account-name", RegexEPasaParser.PasaPattern, Tuple.Create("AccountName", "my-account-name"));
		}

		#endregion

		#region Payload

		[Test]
		public void Payload_Ascii() {
			AssertEx.RegexMatch(@"[""hello world!""]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
			AssertEx.RegexMatch(@"(""hello world!"")", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "("), Tuple.Create("PayloadEndChar", ")"));
			AssertEx.RegexMatch(@"<""hello world!"">", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "<"), Tuple.Create("PayloadEndChar", ">"));
			AssertEx.RegexMatch(@"{""hello world!"":password}", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "{"), Tuple.Create("PayloadEndChar", "}"), Tuple.Create("PayloadPassword", "password"), Tuple.Create("PayloadPasswordDelim", ":"));
		}

		[Test]
		public void Payload_Hex() {
			AssertEx.RegexMatch("[0x1234]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "0x1234"), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
			AssertEx.RegexMatch("(0x1234)", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "0x1234"), Tuple.Create("PayloadStartChar", "("), Tuple.Create("PayloadEndChar", ")"));
			AssertEx.RegexMatch("<0x1234>", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "0x1234"), Tuple.Create("PayloadStartChar", "<"), Tuple.Create("PayloadEndChar", ">"));
			AssertEx.RegexMatch("{0x1234:password}", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "0x1234"), Tuple.Create("PayloadPassword", "password"), Tuple.Create("PayloadStartChar", "{"), Tuple.Create("PayloadEndChar", "}"));
		}

		[Test]
		public void Payload_Base58() {
			AssertEx.RegexMatch("[B58nde3]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "B58nde3"), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
			AssertEx.RegexMatch("(B58nde3)", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "B58nde3"), Tuple.Create("PayloadStartChar", "("), Tuple.Create("PayloadEndChar", ")"));
			AssertEx.RegexMatch("<B58nde3>", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "B58nde3"), Tuple.Create("PayloadStartChar", "<"), Tuple.Create("PayloadEndChar", ">"));
			AssertEx.RegexMatch("{B58nde3:password}", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "B58nde3"), Tuple.Create("PayloadPassword", "password"), Tuple.Create("PayloadStartChar", "{"), Tuple.Create("PayloadEndChar", "}"));
		}

		[Test]
		public void Payload_Hex_MissingNibble() {
			AssertEx.RegexNotMatch("[0x123]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "0x123"), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
		}

		[Test]
		public void Payload_Hex_WhitespaceErrors() {
			AssertEx.RegexNotMatch(@"[ ""hello world!""]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", @" ""hello world!"""), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
			AssertEx.RegexNotMatch(@"[""hello world!"" ]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", @"""hello world!"" "), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
			AssertEx.RegexNotMatch("[ 0x1234]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", " 0x1234"), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
			AssertEx.RegexNotMatch("[0x1234 ]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "0x1234 "), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
			AssertEx.RegexNotMatch("[ B58nde3]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", " B58nde3"), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
			AssertEx.RegexNotMatch("[B58nde3 ]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "B58nde3 "), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
		}

		[Test]
		public void Payload_EmptyContent() {
			AssertEx.RegexMatch(@"[]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", string.Empty), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
			AssertEx.RegexMatch(@"()", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", string.Empty), Tuple.Create("PayloadStartChar", "("), Tuple.Create("PayloadEndChar", ")"));
			AssertEx.RegexMatch(@"<>", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", string.Empty), Tuple.Create("PayloadStartChar", "<"), Tuple.Create("PayloadEndChar", ">"));
			AssertEx.RegexMatch(@"{:password}", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", string.Empty), Tuple.Create("PayloadStartChar", "{"), Tuple.Create("PayloadEndChar", "}"), Tuple.Create("PayloadPassword", "password"), Tuple.Create("PayloadPasswordDelim", ":"));
		}

		[Test]
		public void Payload_EmptyPassword() {
			AssertEx.RegexMatch(@"{:}", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", string.Empty), Tuple.Create("PayloadStartChar", "{"), Tuple.Create("PayloadEndChar", "}"), Tuple.Create("PayloadPassword", string.Empty), Tuple.Create("PayloadPasswordDelim", ":"));
		}

		[Test]
		public void Payload_MissingPasswordDelimiter() {
			AssertEx.RegexMatch(@"{}", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", string.Empty), Tuple.Create("PayloadStartChar", "{"), Tuple.Create("PayloadEndChar", "}"), Tuple.Create("PayloadPassword", string.Empty), Tuple.Create("PayloadPasswordDelim", null as string));
		}

		#endregion

		#region ExtendedChecksum

		[Test]
		public void ExtendedChecksum_Valid() {
			AssertEx.RegexMatch(":ffee", RegexEPasaParser.ExtendedChecksumPattern, Tuple.Create("ExtendedChecksum", "ffee"), Tuple.Create("ExtendedChecksumDelim", ":"));
		}

		[Test]
		public void ExtendedChecksum_MissingDelim() {
			AssertEx.RegexNotMatch("ffee", RegexEPasaParser.ExtendedChecksumPattern, Tuple.Create("ExtendedChecksum", "ffee"), Tuple.Create("ExtendedChecksumDelim", null as string));
		}

		[Test]
		public void ExtendedChecksum_TooLong() {
			AssertEx.RegexNotMatch(":ffeee", RegexEPasaParser.ExtendedChecksumPattern, Tuple.Create("ExtendedChecksum", "ffeee"), Tuple.Create("ExtendedChecksumDelim", ":"));
		}

		[Test]
		public void ExtendedChecksum_TooShort() {
			AssertEx.RegexNotMatch(":ffe", RegexEPasaParser.ExtendedChecksumPattern, Tuple.Create("ExtendedChecksum", "ffe"), Tuple.Create("ExtendedChecksumDelim", ":"));
		}


		[Test]
		public void ExtendedChecksum_Empty() {
			AssertEx.RegexMatch("", RegexEPasaParser.PasaPattern, Tuple.Create("ExtendedChecksumDelim", null as string));
		}


		#endregion

		#region EPasa

		[Test]
		public void EPasa_Full() {
			AssertEx.RegexMatch(@"77-44[""hello world!""]:ffee", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("ChecksumDelim", "-"), Tuple.Create("Checksum", "44"), Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"), Tuple.Create("ExtendedChecksum", "ffee"), Tuple.Create("ExtendedChecksumDelim", ":"));
			AssertEx.RegexMatch(@"(""hello world!"")", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "("), Tuple.Create("PayloadEndChar", ")"));
			AssertEx.RegexMatch(@"<""hello world!"">", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "<"), Tuple.Create("PayloadEndChar", ">"));
			AssertEx.RegexMatch(@"{""hello world!"":password}", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "{"), Tuple.Create("PayloadEndChar", "}"), Tuple.Create("PayloadPassword", "password"), Tuple.Create("PayloadPasswordDelim", ":"));
		}

		[Test]
		public void EPasa_WithoutAccountChecksum() {
			AssertEx.RegexMatch(@"77[""hello world!""]:ffee", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("ChecksumDelim", null as string), Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"), Tuple.Create("ExtendedChecksum", "ffee"), Tuple.Create("ExtendedChecksumDelim", ":"));
		}

		[Test]
		public void EPasa_WithoutExtendedChecksum() {
			AssertEx.RegexMatch(@"77-44[""hello world!""]", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("ChecksumDelim", "-"), Tuple.Create("Checksum", "44"), Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"), Tuple.Create("ExtendedChecksum", null as string), Tuple.Create("ExtendedChecksumDelim", null as string));
		}

		[Test]
		public void EPasa_WithoutChecksums() {
			AssertEx.RegexMatch(@"77[""hello world!""]", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("ChecksumDelim", null as string), Tuple.Create("Checksum", null as string), Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"), Tuple.Create("ExtendedChecksum", null as string), Tuple.Create("ExtendedChecksumDelim", null as string));
			AssertEx.RegexMatch(@"my-account-name[""hello world!""]", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountName", "my-account-name"), Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"), Tuple.Create("ExtendedChecksum", null as string), Tuple.Create("ExtendedChecksumDelim", null as string));
		}

		[Test]
		public void EPasa_EmptyPayloadNoChecksums() {
			AssertEx.RegexMatch(@"77[]", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("ChecksumDelim", null as string), Tuple.Create("Checksum", null as string), Tuple.Create("PayloadContent", null as string), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"), Tuple.Create("ExtendedChecksum", null as string), Tuple.Create("ExtendedChecksumDelim", null as string));
			AssertEx.RegexMatch(@"my-account-name[]", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountName", "my-account-name"), Tuple.Create("PayloadContent", null as string), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"), Tuple.Create("ExtendedChecksum", null as string), Tuple.Create("ExtendedChecksumDelim", null as string));
		}

		[Test]
		public void EPasa_EmptyPayloadNoAccountChecksum() {
			AssertEx.RegexMatch(@"77[]:ffee", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("ChecksumDelim", null as string), Tuple.Create("Checksum", null as string), Tuple.Create("PayloadContent", null as string), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"), Tuple.Create("ExtendedChecksum", "ffee"), Tuple.Create("ExtendedChecksumDelim", ":"));
		}

		[Test]
		public void EPasa_PasaOnly() {
			AssertEx.RegexMatch(@"77", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("ChecksumDelim", null as string), Tuple.Create("Checksum", null as string), Tuple.Create("PayloadContent", null as string), Tuple.Create("PayloadStartChar", null as string), Tuple.Create("PayloadEndChar", null as string), Tuple.Create("ExtendedChecksum", null as string), Tuple.Create("ExtendedChecksumDelim", null as string));
			AssertEx.RegexMatch(@"77-44", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("ChecksumDelim", "-"), Tuple.Create("Checksum", "44"), Tuple.Create("PayloadContent", null as string), Tuple.Create("PayloadStartChar", null as string), Tuple.Create("PayloadEndChar", null as string), Tuple.Create("ExtendedChecksum", null as string), Tuple.Create("ExtendedChecksumDelim", null as string));
			AssertEx.RegexMatch(@"my-account-name", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountName", "my-account-name"), Tuple.Create("PayloadContent", null as string), Tuple.Create("PayloadStartChar", null as string), Tuple.Create("PayloadEndChar", null as string), Tuple.Create("ExtendedChecksum", null as string), Tuple.Create("ExtendedChecksumDelim", null as string));
		}

		[Test]
		public void EPasa_AccountNumberExtendedChecksum() {
			AssertEx.RegexMatch(@"77:ffee", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("ChecksumDelim", null as string), Tuple.Create("Checksum", null as string), Tuple.Create("PayloadContent", null as string), Tuple.Create("PayloadStartChar", null as string), Tuple.Create("PayloadEndChar", null as string), Tuple.Create("ExtendedChecksum", "ffee"), Tuple.Create("ExtendedChecksumDelim", ":"));
			AssertEx.RegexMatch(@"my-account-name:ffee", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountName", "my-account-name"), Tuple.Create("PayloadContent", null as string), Tuple.Create("PayloadStartChar", null as string), Tuple.Create("PayloadEndChar", null as string), Tuple.Create("ExtendedChecksum", "ffee"), Tuple.Create("ExtendedChecksumDelim", ":"));
		}

		#endregion
	}
}

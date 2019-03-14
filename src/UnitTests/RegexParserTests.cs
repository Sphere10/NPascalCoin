using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NPascalCoin.Common.Parsing;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Sphere10.Framework;

namespace Tests {
	public class RegexParserTests {
		[SetUp]
		public void Setup() {
		}

		#region SafeAnsiChar 

		[Test]
		public void SafeAnsiCharPattern_Unescaped() {
			foreach (var ch in EncodingHelper.SafeAnsiCharSetUnescaped)
				AssertRegexMatch($"{ch}", RegexEPasaParser.SafeAnsiCharPattern);
		}

		[Test]
		public void SafeAnsiCharPattern_Escaped() {
			foreach (var ch in EncodingHelper.SafeAnsiCharSetEscaped)
				AssertRegexMatch($@"\{ch}", RegexEPasaParser.SafeAnsiCharPattern);
		}

		[Test]
		public void SafeAnsiCharPattern_IllegalEscape() {
			foreach (var ch in EncodingHelper.SafeAnsiCharSetUnescaped)
				AssertRegexNotMatch($"{EncodingHelper.EscapeChar}{ch}", RegexEPasaParser.SafeAnsiCharPattern);
		}

		[Test]
		public void SafeAnsiCharPattern_MissingEscape() {
			foreach (var ch in EncodingHelper.SafeAnsiCharSetEscaped)
				AssertRegexNotMatch($"{ch}", RegexEPasaParser.SafeAnsiCharPattern);
		}

		[Test]
		public void SafeAnsiCharPattern_IllegalChar() {
			var illegalCharSet = Enumerable
				.Range(0, 255)
				.Select(x => Encoding.ASCII.GetString(new[] {(byte) x})[0])
				.Where(c => !EncodingHelper.SafeAnsiCharSet.Contains(c));

			foreach (var ch in illegalCharSet)
				AssertRegexNotMatch($"{ch}", RegexEPasaParser.SafeAnsiCharPattern);
		}

		#endregion

		#region Pascal64Char

		[Test]
		public void Pascal64StartChar_Unescaped() {
			foreach (var ch in EncodingHelper.Pascal64CharSetStart.Where(c => !EncodingHelper.Pascal64CharSetEscaped.Contains(c)))
				AssertRegexMatch($"{ch}", RegexEPasaParser.Pascal64StartCharPattern);
		}

		[Test]
		public void Pascal64StartChar_Escaped() {
			foreach (var ch in EncodingHelper.Pascal64CharSetStart.Where(c => EncodingHelper.Pascal64CharSetEscaped.Contains(c)))
				AssertRegexMatch($@"\{ch}", RegexEPasaParser.Pascal64StartCharPattern);
		}

		[Test]
		public void Pascal64StartChar_IllegalEscape() {
			foreach (var ch in EncodingHelper.Pascal64CharSetStart.Where(c => !EncodingHelper.Pascal64CharSetEscaped.Contains(c)))
				AssertRegexNotMatch($@"\{ch}", RegexEPasaParser.Pascal64StartCharPattern);
		}

		[Test]
		public void Pascal64StartChar_MissingEscape() {
			foreach (var ch in EncodingHelper.Pascal64CharSetStart.Where(c => EncodingHelper.Pascal64CharSetEscaped.Contains(c)))
				AssertRegexNotMatch($"{ch}", RegexEPasaParser.Pascal64StartCharPattern);
		}

		[Test]
		public void Pascal64StartChar_IllegalChar() {
			var illegalCharSet = Enumerable
				.Range(0, 255)
				.Select(x => Encoding.ASCII.GetString(new[] { (byte)x })[0])
				.Where(c => !EncodingHelper.Pascal64CharSetStart.Contains(c));

			foreach (var ch in illegalCharSet)
				AssertRegexNotMatch($"{ch}", RegexEPasaParser.Pascal64StartCharPattern);
		}

		[Test]
		public void Pascal64NextChar_Unescaped() {
			foreach (var ch in EncodingHelper.Pascal64CharSetUnescaped)
				AssertRegexMatch($"{ch}", RegexEPasaParser.Pascal64NextCharPattern);
		}

		[Test]
		public void Pascal64NextChar_Escaped() {
			foreach (var ch in EncodingHelper.Pascal64CharSetEscaped)
				AssertRegexMatch($@"\{ch}", RegexEPasaParser.Pascal64NextCharPattern);
		}

		[Test]
		public void Pascal64NextChar_IllegalEscape() {
			foreach (var ch in EncodingHelper.Pascal64CharSetUnescaped)
				AssertRegexNotMatch($@"\{ch}", RegexEPasaParser.Pascal64NextCharPattern);
		}

		[Test]
		public void Pascal64NextChar_MissingEscape() {
			foreach (var ch in EncodingHelper.Pascal64CharSetEscaped)
				AssertRegexNotMatch($"{ch}", RegexEPasaParser.Pascal64NextCharPattern);
		}

		[Test]
		public void Pascal64NextChar_IllegalChar() {
			var illegalCharSet = Enumerable
				.Range(0, 255)
				.Select(x => Encoding.ASCII.GetString(new[] { (byte)x })[0])
				.Where(c => !EncodingHelper.Pascal64CharSet.Contains(c));

			foreach (var ch in illegalCharSet)
				AssertRegexNotMatch($"{ch}", RegexEPasaParser.Pascal64NextCharPattern);
		}

		#endregion

		#region SafeAnsiString

		[Test]
		public void SafeAnsiStringPattern_Unescaped() {
			AssertRegexMatch(EncodingHelper.SafeAnsiCharSetUnescaped, RegexEPasaParser.SafeAnsiStringPattern);
		}

		[Test]
		public void SafeAnsiStringPattern_Escaped() {
			var input = @"proper\\escape";
			AssertRegexMatch(input, RegexEPasaParser.SafeAnsiStringPattern);
		}

		[Test]
		public void SafeAnsiStringPattern_FullCharSet_Escaped() {
			AssertRegexMatch(EncodingHelper.EscapeUnescapedString(EncodingHelper.SafeAnsiCharSet), RegexEPasaParser.SafeAnsiStringPattern);
		}

		[Test]
		public void SafeAnsiStringPattern_IllegalEscape() {
			var badInput = @"illegal\escape";
			AssertRegexNotMatch(badInput, RegexEPasaParser.SafeAnsiStringPattern);
		}

		[Test]
		public void SafeAnsiStringPattern_MissingEscape() {
			var badInput = @"missing[]escape";
			AssertRegexNotMatch(badInput, RegexEPasaParser.SafeAnsiStringPattern);
		}

		[Test]
		public void SafeAnsiStringPattern_IllegalCharSet() {
			var illegalCharSet =
				Enumerable
					.Range(0, 255)
					.Select(x => Encoding.ASCII.GetString(new[] {(byte) x})[0])
					.Where(c => !EncodingHelper.SafeAnsiCharSet.Contains(c))
					.ToDelimittedString(string.Empty);			

			AssertRegexNotMatch(illegalCharSet, RegexEPasaParser.SafeAnsiStringPattern);
			AssertRegexNotMatch(EncodingHelper.SafeAnsiCharSetUnescaped + illegalCharSet, RegexEPasaParser.SafeAnsiStringPattern);
			AssertRegexNotMatch(illegalCharSet + EncodingHelper.SafeAnsiCharSetUnescaped, RegexEPasaParser.SafeAnsiStringPattern);
		}

		#endregion

		#region Pascal64Strings

		[Test]
		public void Pasca64StringPattern_Unescaped() {
			AssertRegexMatch(EncodingHelper.Pascal64CharSetUnescaped, RegexEPasaParser.Pascal64StringPattern);
		}

		[Test]
		public void Pascal64StringPattern_Escaped() {
			var badInput = @"correctly\[\]escaped";
			AssertRegexMatch(badInput, RegexEPasaParser.Pascal64StringPattern);
		}

		[Test]
		public void Pascal64StringPattern_FullCharSet_Escaped() {
			AssertRegexMatch(EncodingHelper.EscapeUnescapedString(EncodingHelper.Pascal64CharSet), RegexEPasaParser.Pascal64StringPattern);
		}

		[Test]
		public void Pascal64StringPattern_IllegalEscape() {
			var badInput = @"illegal\escape";
			AssertRegexNotMatch(badInput, RegexEPasaParser.Pascal64StringPattern);
		}

		[Test]
		public void Pascal64StringPattern_MissingEscape() {
			var badInput = @"missing[]escape";
			AssertRegexNotMatch(badInput, RegexEPasaParser.Pascal64StringPattern);
		}

		[Test]
		public void Pascal64StringPattern_IllegalCharSet() {
			var illegalCharSet =
				Enumerable
					.Range(0, 255)
					.Select(x => Encoding.ASCII.GetString(new[] { (byte)x })[0])
					.Where(c => !EncodingHelper.Pascal64CharSet.Contains(c))
					.ToDelimittedString(string.Empty);

			foreach (var subset in illegalCharSet.Partition(63)) {
				var testSet = "b" + new string(subset.ToArray());
				AssertRegexNotMatch(testSet, RegexEPasaParser.Pascal64StringPattern);
			}
		}

		[Test]
		public void Pascal64StringPattern_TooShort() {
			var badInput = @"bb";
			AssertRegexNotMatch(badInput, RegexEPasaParser.Pascal64StringPattern);
		}

		[Test]
		public void Pascal64StringPattern_TooLong() {
			var badInput = Enumerable.Range(0, 65).Select(c => 'b').ToDelimittedString(string.Empty);
			AssertRegexNotMatch(badInput, RegexEPasaParser.Pascal64StringPattern);
		}


		#endregion

		#region Integer

		[Test]
		public void IntegerPattern_Standard() {			
			AssertRegexMatch("1234567890", RegexEPasaParser.IntegerPattern);
		}

		[Test]
		public void IntegerPattern_Starts0() {
			AssertRegexNotMatch("01234567890", RegexEPasaParser.IntegerPattern);
		}

		[Test]
		public void IntegerPattern_Space() {
			AssertRegexNotMatch("12 34567890", RegexEPasaParser.IntegerPattern);
		}

		#endregion

		#region Hex

		[Test]
		public void HexNibble_Valid() {
			foreach (var ch in EncodingHelper.HexStringCharSet) 
				AssertRegexMatch($"{ch}", RegexEPasaParser.HexNibblePattern);			
		}

		[Test]
		public void HexNibble_Illegal() {
			var illegalCharSet = Enumerable
				.Range(0, 255)
				.Select(x => Encoding.ASCII.GetString(new[] { (byte)x })[0])
				.Where(c => !EncodingHelper.HexStringCharSet.Contains(c));

			foreach (var ch in illegalCharSet)
				AssertRegexNotMatch($"{ch}", RegexEPasaParser.HexNibblePattern);
		}

		[Test]
		public void HexByte_Valid() {
			foreach (var n1 in EncodingHelper.HexStringCharSet)
				foreach (var n2 in EncodingHelper.HexStringCharSet)
					AssertRegexMatch($"{n1}{n2}", RegexEPasaParser.HexBytePattern);
		}

		[Test]
		public void HexByte_TooShort() {
			AssertRegexNotMatch("f", RegexEPasaParser.HexBytePattern);
		}

		[Test]
		public void HexByte_TooLong() {
			AssertRegexNotMatch("abf", RegexEPasaParser.HexBytePattern);
		}

		[Test]
		public void HexString_FullCharSet() {
			AssertRegexMatch(EncodingHelper.HexStringCharSet, RegexEPasaParser.HexStringPattern);
		}

		#endregion

		#region Base58

		[Test]
		public void Base58Char_Valid() {
			foreach (var ch in EncodingHelper.Base58CharSet)
				AssertRegexMatch($"{ch}", RegexEPasaParser.Base58CharPattern);
		}

		[Test]
		public void Base58Char_Illegal() {
			var illegalCharSet = Enumerable
				.Range(0, 255)
				.Select(x => Encoding.ASCII.GetString(new[] { (byte)x })[0])
				.Where(c => !EncodingHelper.Base58CharSet.Contains(c));

			foreach (var ch in illegalCharSet)
				AssertRegexNotMatch($"{ch}", RegexEPasaParser.Base58CharPattern);
		}

		[Test]
		public void Base58String_FullCharSet() {
			AssertRegexMatch(EncodingHelper.Base58CharSet, RegexEPasaParser.Base58StringPattern);
		}

		#endregion

		#region AccountNumber

		[Test]
		public void AccountNumber_Full() {			
			AssertRegexMatch("77-44", RegexEPasaParser.AccountNumberPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("Checksum", "44"), Tuple.Create("ChecksumDelim", "-"));
		}

		[Test]
		public void AccountNumber_Whitespace() {
			AssertRegexNotMatch("77- 44", RegexEPasaParser.AccountNumberPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("Checksum", "44"), Tuple.Create("ChecksumDelim", "-"));
		}

		[Test]
		public void AccountNumber_NoChecksum() {
			AssertRegexMatch("77", RegexEPasaParser.AccountNumberPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("Checksum", null as string), Tuple.Create("ChecksumDelim", null as string));
		}

		[Test]
		public void AccountNumber_MalformedChecksum() {
			// not expecting delim
			AssertRegexNotMatch("77-", RegexEPasaParser.AccountNumberPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("Checksum", null as string), Tuple.Create("ChecksumDelim", "-"));
		}

		#endregion

		#region PASA 

		[Test]
		public void PASA_AccountNumber() {
			AssertRegexMatch("77-44", RegexEPasaParser.PasaPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("Checksum", "44"), Tuple.Create("ChecksumDelim", "-"));
		}

		[Test]
		public void PASA_AccountName() {
			AssertRegexMatch("my-account-name", RegexEPasaParser.PasaPattern, Tuple.Create("AccountName", "my-account-name"));
		}

		#endregion

		#region Payload

		[Test]
		public void Payload_Ascii() {
			AssertRegexMatch(@"[""hello world!""]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
			AssertRegexMatch(@"(""hello world!"")", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "("), Tuple.Create("PayloadEndChar", ")"));
			AssertRegexMatch(@"<""hello world!"">", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "<"), Tuple.Create("PayloadEndChar", ">"));
			AssertRegexMatch(@"{""hello world!"":password}", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "{"), Tuple.Create("PayloadEndChar", "}"), Tuple.Create("PayloadPassword", "password"), Tuple.Create("PayloadPasswordDelim", ":"));
		}

		[Test]
		public void Payload_Hex() {
			AssertRegexMatch("[0x1234]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "0x1234"), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
			AssertRegexMatch("(0x1234)", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "0x1234"), Tuple.Create("PayloadStartChar", "("), Tuple.Create("PayloadEndChar", ")"));
			AssertRegexMatch("<0x1234>", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "0x1234"), Tuple.Create("PayloadStartChar", "<"), Tuple.Create("PayloadEndChar", ">"));
			AssertRegexMatch("{0x1234:password}", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "0x1234"), Tuple.Create("PayloadPassword", "password"), Tuple.Create("PayloadStartChar", "{"), Tuple.Create("PayloadEndChar", "}"));
		}

		[Test]
		public void Payload_Base58() {
			AssertRegexMatch("[B58nde3]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "B58nde3"), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
			AssertRegexMatch("(B58nde3)", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "B58nde3"), Tuple.Create("PayloadStartChar", "("), Tuple.Create("PayloadEndChar", ")"));
			AssertRegexMatch("<B58nde3>", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "B58nde3"), Tuple.Create("PayloadStartChar", "<"), Tuple.Create("PayloadEndChar", ">"));
			AssertRegexMatch("{B58nde3:password}", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "B58nde3"), Tuple.Create("PayloadPassword", "password"), Tuple.Create("PayloadStartChar", "{"), Tuple.Create("PayloadEndChar", "}"));
		}

		[Test]
		public void Payload_Hex_MissingNibble() {
			AssertRegexNotMatch("[0x123]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "0x123"), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
		}

		[Test]
		public void Payload_Hex_WhitespaceErrors() {
			AssertRegexNotMatch(@"[ ""hello world!""]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", @" ""hello world!"""), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
			AssertRegexNotMatch(@"[""hello world!"" ]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", @"""hello world!"" "), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
			AssertRegexNotMatch("[ 0x1234]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", " 0x1234"), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
			AssertRegexNotMatch("[0x1234 ]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "0x1234 "), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
			AssertRegexNotMatch("[ B58nde3]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", " B58nde3"), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
			AssertRegexNotMatch("[B58nde3 ]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", "B58nde3 "), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
		}

		[Test]
		public void Payload_EmptyContent() {
			AssertRegexMatch(@"[]", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", string.Empty), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"));
			AssertRegexMatch(@"()", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", string.Empty), Tuple.Create("PayloadStartChar", "("), Tuple.Create("PayloadEndChar", ")"));
			AssertRegexMatch(@"<>", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", string.Empty), Tuple.Create("PayloadStartChar", "<"), Tuple.Create("PayloadEndChar", ">"));
			AssertRegexMatch(@"{:password}", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", string.Empty), Tuple.Create("PayloadStartChar", "{"), Tuple.Create("PayloadEndChar", "}"), Tuple.Create("PayloadPassword", "password"), Tuple.Create("PayloadPasswordDelim", ":"));
		}

		[Test]
		public void Payload_EmptyPassword() {
			AssertRegexMatch(@"{:}", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", string.Empty), Tuple.Create("PayloadStartChar", "{"), Tuple.Create("PayloadEndChar", "}"), Tuple.Create("PayloadPassword", string.Empty), Tuple.Create("PayloadPasswordDelim", ":"));
		}

		[Test]
		public void Payload_MissingPasswordDelimiter() {
			AssertRegexMatch(@"{}", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", string.Empty), Tuple.Create("PayloadStartChar", "{"), Tuple.Create("PayloadEndChar", "}"), Tuple.Create("PayloadPassword", string.Empty), Tuple.Create("PayloadPasswordDelim", null as string));
		}

		#endregion

		#region ExtendedChecksum

		[Test]
		public void ExtendedChecksum_Valid() {
			AssertRegexMatch(":ffee", RegexEPasaParser.ExtendedChecksumPattern, Tuple.Create("ExtendedChecksum", "ffee"), Tuple.Create("ExtendedChecksumDelim", ":"));
		}

		[Test]
		public void ExtendedChecksum_MissingDelim() {
			AssertRegexNotMatch("ffee", RegexEPasaParser.ExtendedChecksumPattern, Tuple.Create("ExtendedChecksum", "ffee"), Tuple.Create("ExtendedChecksumDelim", null as string));
		}

		[Test]
		public void ExtendedChecksum_TooLong() {
			AssertRegexNotMatch(":ffeee", RegexEPasaParser.ExtendedChecksumPattern, Tuple.Create("ExtendedChecksum", "ffeee"), Tuple.Create("ExtendedChecksumDelim", ":"));
		}

		[Test]
		public void ExtendedChecksum_TooShort() {
			AssertRegexNotMatch(":ffe", RegexEPasaParser.ExtendedChecksumPattern, Tuple.Create("ExtendedChecksum", "ffe"), Tuple.Create("ExtendedChecksumDelim", ":"));
		}


		[Test]
		public void ExtendedChecksum_Empty() {
			AssertRegexMatch("", RegexEPasaParser.PasaPattern, Tuple.Create("ExtendedChecksumDelim", null as string));
		}


		#endregion

		#region EPasa

		[Test]
		public void EPasa_Full() {
			AssertRegexMatch(@"77-44[""hello world!""]:ffee", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("ChecksumDelim", "-"), Tuple.Create("Checksum", "44"), Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"), Tuple.Create("ExtendedChecksum", "ffee"), Tuple.Create("ExtendedChecksumDelim", ":"));
			//AssertRegexMatch(@"(""hello world!"")", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "("), Tuple.Create("PayloadEndChar", ")"));
			//AssertRegexMatch(@"<""hello world!"">", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "<"), Tuple.Create("PayloadEndChar", ">"));
			//AssertRegexMatch(@"{""hello world!"":password}", RegexEPasaParser.PayloadPattern, Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "{"), Tuple.Create("PayloadEndChar", "}"), Tuple.Create("PayloadPassword", "password"), Tuple.Create("PayloadPasswordDelim", ":"));
		}

		[Test]
		public void EPasa_WithoutAccountChecksum() {
			AssertRegexMatch(@"77[""hello world!""]:ffee", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("ChecksumDelim", null as string),  Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"), Tuple.Create("ExtendedChecksum", "ffee"), Tuple.Create("ExtendedChecksumDelim", ":"));
		}

		[Test]
		public void EPasa_WithoutExtendedChecksum() {
			AssertRegexMatch(@"77-44[""hello world!""]", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("ChecksumDelim", "-"), Tuple.Create("Checksum", "44"), Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"), Tuple.Create("ExtendedChecksum", null as string), Tuple.Create("ExtendedChecksumDelim", null as string));
		}

		[Test]
		public void EPasa_WithoutChecksums() {
			AssertRegexMatch(@"77[""hello world!""]", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("ChecksumDelim", null as string), Tuple.Create("Checksum", null as string), Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"), Tuple.Create("ExtendedChecksum", null as string), Tuple.Create("ExtendedChecksumDelim", null as string));
			AssertRegexMatch(@"my-account-name[""hello world!""]", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountName", "my-account-name"), Tuple.Create("PayloadContent", @"""hello world!"""), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"), Tuple.Create("ExtendedChecksum", null as string), Tuple.Create("ExtendedChecksumDelim", null as string));
		}

		[Test]
		public void EPasa_EmptyPayloadNoChecksums() {
			AssertRegexMatch(@"77[]", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("ChecksumDelim", null as string), Tuple.Create("Checksum", null as string), Tuple.Create("PayloadContent", null as string), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"), Tuple.Create("ExtendedChecksum", null as string), Tuple.Create("ExtendedChecksumDelim", null as string));
			AssertRegexMatch(@"my-account-name[]", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountName", "my-account-name"), Tuple.Create("PayloadContent", null as string), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"), Tuple.Create("ExtendedChecksum", null as string), Tuple.Create("ExtendedChecksumDelim", null as string));
		}

		[Test]
		public void EPasa_EmptyPayloadNoAccountChecksum() {
			AssertRegexMatch(@"77[]:ffee", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("ChecksumDelim", null as string), Tuple.Create("Checksum", null as string), Tuple.Create("PayloadContent", null as string), Tuple.Create("PayloadStartChar", "["), Tuple.Create("PayloadEndChar", "]"), Tuple.Create("ExtendedChecksum", "ffee"), Tuple.Create("ExtendedChecksumDelim", ":"));
		}

		[Test]
		public void EPasa_PasaOnly() {
			AssertRegexMatch(@"77", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("ChecksumDelim", null as string), Tuple.Create("Checksum", null as string), Tuple.Create("PayloadContent", null as string), Tuple.Create("PayloadStartChar", null as string), Tuple.Create("PayloadEndChar", null as string), Tuple.Create("ExtendedChecksum", null as string), Tuple.Create("ExtendedChecksumDelim", null as string));
			AssertRegexMatch(@"77-44", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("ChecksumDelim", "-"), Tuple.Create("Checksum", "44"), Tuple.Create("PayloadContent", null as string), Tuple.Create("PayloadStartChar", null as string), Tuple.Create("PayloadEndChar", null as string), Tuple.Create("ExtendedChecksum", null as string), Tuple.Create("ExtendedChecksumDelim", null as string));
			AssertRegexMatch(@"my-account-name", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountName", "my-account-name"),  Tuple.Create("PayloadContent", null as string), Tuple.Create("PayloadStartChar", null as string), Tuple.Create("PayloadEndChar", null as string), Tuple.Create("ExtendedChecksum", null as string), Tuple.Create("ExtendedChecksumDelim", null as string));
		}

		[Test]
		public void EPasa_AccountNumberExtendedChecksum() {
			AssertRegexMatch(@"77:ffee", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountNumber", "77"), Tuple.Create("ChecksumDelim", null as string), Tuple.Create("Checksum", null as string), Tuple.Create("PayloadContent", null as string), Tuple.Create("PayloadStartChar", null as string), Tuple.Create("PayloadEndChar", null as string), Tuple.Create("ExtendedChecksum", "ffee"), Tuple.Create("ExtendedChecksumDelim", ":"));
			AssertRegexMatch(@"my-account-name:ffee", RegexEPasaParser.EPasaPattern, Tuple.Create("AccountName", "my-account-name"), Tuple.Create("PayloadContent", null as string), Tuple.Create("PayloadStartChar", null as string), Tuple.Create("PayloadEndChar", null as string), Tuple.Create("ExtendedChecksum", "ffee"), Tuple.Create("ExtendedChecksumDelim", ":"));
		}

		#endregion

		// Aux

		public void AssertRegexMatch(string expected, string regexPattern, params Tuple<string, string>[] expectedCaptures) {
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

		public void AssertRegexNotMatch(string badInput, string regexPattern, params Tuple<string, string>[] expectedCaptures) {
			var regex = new Regex(regexPattern);
			var match = regex.Match(badInput);
			Assert.AreNotEqual(badInput, match.Value);
			if (expectedCaptures.Any())
			  Assert.IsFalse(expectedCaptures.All(c => c.Item2 == match.Groups[c.Item1]?.Value));

		}
	}
}
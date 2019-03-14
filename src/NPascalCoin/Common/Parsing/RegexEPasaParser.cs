using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NPascalCoin.API.Classic.Objects;
using NPascalCoin.Common;

namespace NPascalCoin.Common.Parsing {
	public partial class RegexEPasaParser : IEPasaParser {
		// note: regex syntax escapes following chars [\^$.|?*+(){}
		// note: epasa syntax escapes following chars: :\"[]()<>(){}
		// note: c-sharp syntax verbatim strings escape: " as ""
		public const string SafeAnsiCharPattern = @"( |!|\\""|#|\$|%|&|'|\\\(|\\\)|\*|\+|,|-|\.|/|0|1|2|3|4|5|6|7|8|9|\\:|;|\\<|=|\\>|\?|@|A|B|C|D|E|F|G|H|I|J|K|L|M|N|O|P|Q|R|S|T|U|V|W|X|Y|Z|\\\[|\\\\|\\]|\^|_|`|a|b|c|d|e|f|g|h|i|j|k|l|m|n|o|p|q|r|s|t|u|v|w|x|y|z|\\\{|\||\\\}|~)";
		public const string SafeAnsiStringPattern = SafeAnsiCharPattern + "+";
		public const string Pascal64StartCharPattern = @"(a|b|c|d|e|f|g|h|i|j|k|l|m|n|o|p|q|r|s|t|u|v|w|x|y|z|!|@|#|\$|%|\^|&|\*|\\\(|\\\)|-|\+|\\\{|\\\}|\\\[|\\]|_|\\:|`|\||\\<|\\>|,|\.|\?|/|~)";
		public const string Pascal64NextCharPattern = @"(a|b|c|d|e|f|g|h|i|j|k|l|m|n|o|p|q|r|s|t|u|v|w|x|y|z|0|1|2|3|4|5|6|7|8|9|!|@|#|\$|%|\^|&|\*|\\\(|\\\)|-|\+|\\\{|\\\}|\\\[|\\]|_|\\:|`|\||\\<|\\>|,|\.|\?|/|~)";
		public const string Pascal64StringPattern = Pascal64StartCharPattern + Pascal64NextCharPattern + "{2,63}";
		public const string HexNibblePattern = @"[0-9a-f]";
		public const string HexBytePattern = HexNibblePattern + "{2}";
		public const string HexStringPattern = "(?:" + HexBytePattern + ")+";
		public const string Base58CharPattern = "[123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz]";
		public const string Base58StringPattern =  Base58CharPattern + "+";
		public const string IntegerPattern = @"[1-9]\d+";
		public const string AccountNamePattern = @"(?<AccountName>" + Pascal64StringPattern + ")";
		public const string AccountChecksumPattern = @"(?:(?<ChecksumDelim>-)(?<Checksum>\d{2}))?";
		public const string AccountNumberPattern = "(?<AccountNumber>" + IntegerPattern + ")" + AccountChecksumPattern ;
		public const string PasaPattern = "(" + AccountNumberPattern + "|" + AccountNamePattern + ")";
		public const string ASCIIContentPattern = @"""" + SafeAnsiStringPattern + @"""";
		public const string HexContentPattern =  "0x" + HexStringPattern;
		public const string Base58ContentPattern = Base58StringPattern;
		public const string PayloadPasswordPattern = "(?:(?<PayloadPasswordDelim>" + ":){1}(?<PayloadPassword>" + SafeAnsiStringPattern + ")?)?";
		public const string PayloadStartCharPattern = @"(?<PayloadStartChar>[\[\(<\{])";
		public const string PayloadEndCharPattern = @"(?<PayloadEndChar>[]\)>\}])";
		public const string PayloadContentPattern = "(?<PayloadContent>" + ASCIIContentPattern + "|" + HexContentPattern + "|" + Base58ContentPattern + ")?";
		public const string PayloadPattern = "(?:" + PayloadStartCharPattern + PayloadContentPattern + PayloadPasswordPattern + PayloadEndCharPattern + ")?";
		public const string ExtendedChecksumPattern = "(?:" + "(?<ExtendedChecksumDelim>:)" + "(?<ExtendedChecksum>" + HexBytePattern + HexBytePattern +"))?";
		public const string EPasaPattern = PasaPattern + PayloadPattern + ExtendedChecksumPattern;


		private static readonly Regex _epasaRegex;

		static RegexEPasaParser() {
			_epasaRegex = new Regex(EPasaPattern);
		}
		
		public bool TryParse(string epasaText, out EPasa epasa, out EPasaErrorCode errorCode) {
			errorCode = EPasaErrorCode.Success;
			epasa = new EPasa();

			if (string.IsNullOrEmpty(epasaText)) {
				errorCode = EPasaErrorCode.BadFormat;
				return false;
			}

			var match = _epasaRegex.Match(epasaText);
			var checksumDelim = match.Groups["ChecksumDelim"].Success ? match.Groups["ChecksumDelim"].Value : null;
			var accountNumber = match.Groups["AccountNumber"].Success ? match.Groups["AccountNumber"].Value : null;
			var accountChecksum = match.Groups["AccountChecksum"].Success ? match.Groups["AccountChecksum"].Value : null;
			var accountName = match.Groups["AccountName"].Success ? match.Groups["AccountName"].Value : null;
			var payloadStartChar = match.Groups["PayloadStartChar"].Success ? match.Groups["PayloadStartChar"].Value : null;
			var payloadEndChar = match.Groups["PayloadEndChar"].Success ? match.Groups["PayloadEndChar"].Value : null;
			var payloadContent = match.Groups["PayloadContent"].Success ? match.Groups["PayloadContent"].Value : null;
			var payloadPasswordDelim = match.Groups["PayloadPasswordDelim"].Success ? match.Groups["PayloadPasswordDelim"].Value : null;
			var payloadPassword = match.Groups["PayloadPassword"].Success ? match.Groups["PayloadPassword"].Value : null;
			var extendedChecksumDelim = match.Groups["ExtendedChecksumDelim"].Success ? match.Groups["ExtendedChecksumDelim"].Value : null;
			var extendedChecksum = match.Groups["ExtendedChecksum"].Success ? match.Groups["ExtendedChecksum"].Value : null;

			// Check parsed completely
			if (epasaText != match.Value) {
				errorCode = EPasaErrorCode.BadFormat;
				return false;
			}

			if (accountName != null) {
				// Account Name
				if (string.IsNullOrEmpty(accountName)) {
					errorCode = EPasaErrorCode.BadFormat;
					return false;
				}
				epasa.PayloadType = epasa.PayloadType | PayloadType.AddressedByName;
				epasa.AccountName = accountName;
			} else {
				// Account Number
				if (!uint.TryParse(accountNumber, out var accNo)) {
					errorCode = EPasaErrorCode.AccountNumberTooLong;
					return false;
				}
				epasa.PayloadType = epasa.PayloadType ^ PayloadType.AddressedByName;
				epasa.Account = accNo;

				if (checksumDelim != null) {
					if (!uint.TryParse(accountChecksum, out var accChecksum)) {
						errorCode = EPasaErrorCode.AccountChecksumInvalid;
						return false;
					}
					if (!AccountHelper.IsValidAccountChecksum(epasa.Account.Value, accChecksum)) {
						errorCode = EPasaErrorCode.BadChecksum;
						return false;
					}
					epasa.AccountChecksum = accChecksum;
				}
			}

			// Encryption type			
			switch (payloadStartChar) {
				case null:
					break;
				case "[":
					if (payloadEndChar != "]") {
						errorCode = EPasaErrorCode.MismatchedPayloadEncoding;
						return false;
					}
					epasa.PayloadType = epasa.PayloadType | PayloadType.Public;
					break;
				case "(":
					if (payloadEndChar != ")") {
						errorCode = EPasaErrorCode.MismatchedPayloadEncoding;
						return false;
					}
					epasa.PayloadType = epasa.PayloadType | PayloadType.RecipientKeyEncrypted;
					break;
				case "<":
					if (payloadEndChar != ">") {
						errorCode = EPasaErrorCode.MismatchedPayloadEncoding;
						return false;
					}
					epasa.PayloadType = epasa.PayloadType | PayloadType.SenderKeyEncrypted;
					break;
				case "{":
					if (payloadEndChar != "}") {
						errorCode = EPasaErrorCode.MismatchedPayloadEncoding;
						return false;
					}
					epasa.PayloadType = epasa.PayloadType | PayloadType.AESEncrypted;
					break;
				default:
					throw new NotSupportedException($"Unrecognized start character '{payloadStartChar}'");

			}
 
			// Password
			if (epasa.PayloadType.HasFlag(PayloadType.AESEncrypted)) {
				if (payloadPasswordDelim == null) {
					errorCode = EPasaErrorCode.MissingPassword;
					return false;
				}
				epasa.Password = payloadPassword ?? "";
			} else if (payloadPasswordDelim != null) {
				errorCode = EPasaErrorCode.UnusedPassword;
				return false;
			}

			// Payload 
			if (payloadStartChar != null) {
				if (payloadContent == null) {
					epasa.Payload = string.Empty;
				} else if (payloadContent.StartsWith("\"")) {
					epasa.PayloadType = epasa.PayloadType | PayloadType.AsciiFormatted;
					epasa.Payload = payloadContent.Trim('"');					
				} else if (payloadContent.StartsWith("0x")) {
					epasa.PayloadType = epasa.PayloadType | PayloadType.HexFormatted;
					epasa.Payload = payloadContent.Substring(2);
				} else  {
					epasa.PayloadType = epasa.PayloadType | PayloadType.Base58Formatted;
					epasa.Payload = payloadContent;
				} 
				epasa.Payload = payloadContent;
			}

			// Payload Lengths
			if (!EPasaHelper.IsValidPayloadLength(epasa.PayloadType, epasa.Payload)) {
				errorCode = EPasaErrorCode.PayloadTooLarge;
				return false;
			}

			// Extended Checksum
			if (extendedChecksumDelim != null) {
				if (checksumDelim == null) {
					errorCode = EPasaErrorCode.MissingAccountChecksum;
					return false;
				}
				if (!EPasaHelper.IsValidExtendedChecksum(epasa.ToString(true), epasa.ExtendedChecksum)) {
					errorCode = EPasaErrorCode.BadExtendedChecksum;
					return false;
				}
				epasa.ExtendedChecksum = extendedChecksum;
			}
			return true;
		}
		
	}
}

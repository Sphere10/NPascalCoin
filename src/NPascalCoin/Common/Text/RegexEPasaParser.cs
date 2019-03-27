using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NPascalCoin.API.Classic.Objects;
using NPascalCoin.Common;

namespace NPascalCoin.Common.Text {
	public class RegexEPasaParser : EPasaParserBase {
		// note: regex syntax escapes following chars [\^$.|?*+(){}
		// note: epasa syntax escapes following chars: :\"[]()<>(){}
		// note: c-sharp syntax verbatim strings escape: " as ""
		public const string IntegerPattern = @"(0|[1-9]\d+)";
		public const string AccountNamePattern = @"(?<AccountName>" + Pascal64Encoding.Pascal64StringPattern + ")";
		public const string AccountChecksumPattern = @"(?:(?<ChecksumDelim>-)(?<Checksum>\d{2}))?";
		public const string AccountNumberPattern = "(?<AccountNumber>" + IntegerPattern + ")" + AccountChecksumPattern ;
		public const string PasaPattern = "(" + AccountNumberPattern + "|" + AccountNamePattern + ")";
		public const string ASCIIContentPattern = @"""" + PascalAsciiEncoding.PascalAsciiStringPattern + @"""";
		public const string HexContentPattern = "0x" + HexEncoding.HexStringPattern;
		public const string Base58ContentPattern = PascalBase58Encoding.SubStringPattern;
		public const string PayloadPasswordPattern = "(?:(?<PayloadPasswordDelim>" + ":){1}(?<PayloadPassword>" + PascalAsciiEncoding.PascalAsciiStringPattern + ")?)?";
		public const string PayloadStartCharPattern = @"(?<PayloadStartChar>[\[\(<\{])";
		public const string PayloadEndCharPattern = @"(?<PayloadEndChar>[]\)>\}])";
		public const string PayloadContentPattern = "(?<PayloadContent>" + ASCIIContentPattern + "|" + HexContentPattern + "|" + Base58ContentPattern + ")?";
		public const string PayloadPattern = "(?:" + PayloadStartCharPattern + PayloadContentPattern + PayloadPasswordPattern + PayloadEndCharPattern + ")?";
		public const string ExtendedChecksumPattern = "(?:" + "(?<ExtendedChecksumDelim>:)" + "(?<ExtendedChecksum>" + HexEncoding.HexBytePattern + HexEncoding.HexBytePattern +"))?";
		public const string EPasaPattern = PasaPattern + PayloadPattern + ExtendedChecksumPattern;

		private static readonly Regex _epasaRegex;

		static RegexEPasaParser() {
			_epasaRegex = new Regex(EPasaPattern);
		}
		
		public override bool TryParse(string epasaText, out EPasa epasa, out EPasaErrorCode errorCode) {
			errorCode = EPasaErrorCode.Success;
			epasa = new EPasa();

			if (string.IsNullOrEmpty(epasaText)) {
				errorCode = EPasaErrorCode.BadFormat;
				return false;
			}

			var match = _epasaRegex.Match(epasaText);
			var checksumDelim = match.Groups["ChecksumDelim"].Success ? match.Groups["ChecksumDelim"].Value : null;
			var accountNumber = match.Groups["AccountNumber"].Success ? match.Groups["AccountNumber"].Value : null;
			var accountChecksum = match.Groups["Checksum"].Success ? match.Groups["Checksum"].Value : null;
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
				epasa.AccountName = Pascal64Encoding.Unescape(accountName);
				epasa.Account = epasa.AccountChecksum = null;
			} else {
				// Account Number
				if (!uint.TryParse(accountNumber, out var accNo)) {
					errorCode = EPasaErrorCode.InvalidAccountNumber;
					return false;
				}
				epasa.Account = accNo;
				var actualAccountChecksum = AccountHelper.CalculateAccountChecksum(accNo);

				if (checksumDelim != null) {
					// validate account checksum
					if (!uint.TryParse(accountChecksum, out var accChecksum)) {
						errorCode = EPasaErrorCode.AccountChecksumInvalid;
						return false;
					}
					if (accChecksum != actualAccountChecksum) {
						errorCode = EPasaErrorCode.BadChecksum;
						return false;
					}
				}
				epasa.AccountChecksum = actualAccountChecksum;

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
					epasa.PayloadType = epasa.PayloadType | PayloadType.PasswordEncrypted;
					break;
				default:
					throw new NotSupportedException($"Unrecognized start character '{payloadStartChar}'");

			}
 
			// Password
			if (epasa.PayloadType.HasFlag(PayloadType.PasswordEncrypted)) {
				if (payloadPasswordDelim == null) {
					errorCode = EPasaErrorCode.MissingPassword;
					return false;
				}
				epasa.Password = PascalAsciiEncoding.Unescape(payloadPassword ?? "");
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
					epasa.Payload = PascalAsciiEncoding.Unescape(payloadContent.Trim('"'));
				} else if (payloadContent.StartsWith("0x")) {
					epasa.PayloadType = epasa.PayloadType | PayloadType.HexFormatted;
					epasa.Payload = payloadContent.Substring(2);
				} else  {
					epasa.PayloadType = epasa.PayloadType | PayloadType.Base58Formatted;
					epasa.Payload = payloadContent;
				} 
			}

			// Payload Lengths
			if (!EPasaHelper.IsValidPayloadLength(epasa.PayloadType, epasa.Payload)) {
				errorCode = EPasaErrorCode.PayloadTooLarge;
				return false;
			}

			// Extended Checksum
			var actualChecksum = EPasaHelper.ComputeExtendedChecksum(epasa.ToString(true));
			if (extendedChecksumDelim != null) {
				if (extendedChecksum != actualChecksum) {
					errorCode = EPasaErrorCode.BadExtendedChecksum;
					return false;
				}
			}
			epasa.ExtendedChecksum = actualChecksum;
			return true;
		}
		
	}
}

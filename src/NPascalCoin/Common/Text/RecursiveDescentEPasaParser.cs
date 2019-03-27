using System;
using System.IO;
using System.Linq;
using Sphere10.Framework;

namespace NPascalCoin.Common.Text {

	public class RecursiveDescentEPasaParser : EPasaParserBase {

		public enum TokenType {
			EPASA,
			PASA,
			AccountName,
			AccountNumber,
			Checksum,
			Payload,
			PayloadContent,
			ExtendedChecksum,
			Number,
			PascalAsciiString,
			Pascal64String,
			HexString,
			Base58String
		}

		public abstract class Node {
			public TokenType Type { get; set; }
		}

		public sealed class EPasaNode : Node {
			public AccountNode Account;
			public PayloadNode Payload;
			public ValueNode ExtendedChecksum;
		}

		public sealed class AccountNode : Node {
			public ValueNode AccountNo;
			public ValueNode Checksum;
			public ValueNode Name { get; set; }
		}

		public sealed class PayloadNode : Node {
			public PayloadType Encryption { get; set; }
			public ValueNode Content { get; set; }
			public ValueNode Password { get; set; }
		}

		public sealed class ValueNode : Node {
			public string Value { get; set; }
		}

		public override bool TryParse(string epasaText, out EPasa epasa, out EPasaErrorCode errorCode) {
			errorCode = EPasaErrorCode.Success;
			epasa = new EPasa();
			var node = TryParseEPasaInternal(TokenType.EPASA, new StringReader(epasaText), ref errorCode);
			if (errorCode == EPasaErrorCode.Success) {
				errorCode = TryCompile((EPasaNode)node, epasa);
			}
			return errorCode == EPasaErrorCode.Success;
		}

		private Node TryParseEPasaInternal(TokenType expectedToken, TextReader reader, ref EPasaErrorCode error, string prefix = null, string postFix = null) {
			Node result = null;

			if (error != EPasaErrorCode.Success)
				return null;

			if (prefix != null)
				foreach (var prefixChar in prefix)
					if (!reader.MatchChar(prefixChar)) {
						error = EPasaErrorCode.BadFormat;
						return null;
					}

			switch (expectedToken) {
				case TokenType.EPASA:
					result = new EPasaNode {
						Type = TokenType.EPASA,
						Account = TryParseEPasaInternal(TokenType.PASA, reader, ref error) as AccountNode,
						Payload = TryParseEPasaInternal(TokenType.Payload, reader, ref error) as PayloadNode,
						ExtendedChecksum = TryParseEPasaInternal(TokenType.ExtendedChecksum, reader, ref error) as ValueNode
					};
					if (!reader.MatchChar(null as char?)) {
						// match end-of-string
						error = EPasaErrorCode.BadFormat;
						return null;
					} 
					break;
				case TokenType.PASA:
					if (IsStartChar(TokenType.AccountNumber, reader.PeekChar())) {
						result = TryParseEPasaInternal(TokenType.AccountNumber, reader, ref error);
					} else if (IsStartChar(TokenType.AccountName, reader.PeekChar())) {
						result = TryParseEPasaInternal(TokenType.AccountName, reader, ref error);
					} else {
						error = EPasaErrorCode.InvalidAccountNumber;
					}
					break;
				case TokenType.AccountName:
					result = new AccountNode {
						Type = TokenType.AccountName,
						Name = TryParseEPasaInternal(TokenType.Pascal64String, reader, ref error) as ValueNode
					};
					break;
				case TokenType.AccountNumber:
					result = new AccountNode {
						Type = TokenType.AccountNumber,
						AccountNo = TryParseEPasaInternal(TokenType.Number, reader, ref error) as ValueNode,
						Checksum = TryParseEPasaInternal(TokenType.Checksum, reader, ref error) as ValueNode
					};
					break;
				case TokenType.Checksum:
					if (IsStartChar(TokenType.Checksum, reader.PeekChar()))
						result = TryParseEPasaInternal(TokenType.Number, reader, ref error, prefix: "-") as ValueNode;
					break;
				case TokenType.Payload:
					if (IsStartChar(TokenType.Payload, reader.PeekChar())) {
						result = new PayloadNode {
							Type = TokenType.Payload
						};
						var payloadOpenChar = reader.ReadChar();
						((PayloadNode)result).Content = TryParseEPasaInternal(TokenType.PayloadContent, reader, ref error) as ValueNode;
						if (reader.PeekChar() == ':') {
							if (payloadOpenChar != '{') {
								error = EPasaErrorCode.UnusedPassword;
								return result;
							}
							((PayloadNode)result).Password = TryParseEPasaInternal(TokenType.PascalAsciiString, reader, ref error, prefix: ":") as ValueNode;
						} else if (payloadOpenChar == '{') {
							error = EPasaErrorCode.MissingPassword;
							return null;
						}
						var payloadEndChar = reader.PeekChar();
						if (!payloadEndChar.IsIn(']', ')', '>', '}')) {
							error = EPasaErrorCode.BadFormat;
							return null;
						}
						char? expectedEndChar = null;
						switch (payloadOpenChar) {
							case '[':
								((PayloadNode)result).Encryption = PayloadType.Public;
								expectedEndChar = ']';
								break;
							case '(':
								((PayloadNode)result).Encryption = PayloadType.RecipientKeyEncrypted;
								expectedEndChar = ')';
								break;
							case '<':
								((PayloadNode)result).Encryption = PayloadType.SenderKeyEncrypted;
								expectedEndChar = '>';
								break;
							case '{':
								((PayloadNode)result).Encryption = PayloadType.PasswordEncrypted;
								expectedEndChar = '}';
								break;
							default:
								throw new InternalErrorException("Implementation Error");
						}
						if (reader.PeekChar() != expectedEndChar) {
							error = EPasaErrorCode.MismatchedPayloadEncoding;
							return null;
						}

						if (!reader.MatchChar(']', ')', '>', '}')) {
							error = EPasaErrorCode.BadFormat;
							return null;
						}
					}
					break;
				case TokenType.PayloadContent:
					var contentStartChar = reader.PeekChar();
					if (contentStartChar == '"') {
						result = TryParseEPasaInternal(TokenType.PascalAsciiString, reader, ref error, prefix: "\"", postFix: "\"") as ValueNode;
					} else if (IsStartChar(TokenType.HexString, contentStartChar)) {
						result = TryParseEPasaInternal(TokenType.HexString, reader, ref error, prefix: "0x") as ValueNode;
					} else if (IsStartChar(TokenType.Base58String, contentStartChar)) {
						result = TryParseEPasaInternal(TokenType.Base58String, reader, ref error) as ValueNode;
					} else {
						// unrecognized content, assume empty
					}
					break;
				case TokenType.ExtendedChecksum:
					if (reader.PeekChar() == ':') {
						result = TryParseEPasaInternal(TokenType.HexString, reader, ref error, prefix: ":") as ValueNode;
					}
					break;
				case TokenType.PascalAsciiString:
				case TokenType.Pascal64String:
					result = new ValueNode {
						Type = expectedToken,
						Value = string.Empty
					};
					if (!IsStartChar(expectedToken, reader.PeekChar())) {
						// first character is a non-character, assume empty string
						return result;
					}
					var escapedValue = string.Empty;
					do {
						if (reader.PeekChar() == GetEscapeChar(expectedToken)) {
							escapedValue += reader.ReadChar();
							if (!IsEscapedChar(expectedToken, reader.PeekChar())) {
								error = EPasaErrorCode.BadFormat; // illegal escape sequence
								return result;
							}
						} else if (IsEscapedChar(expectedToken, reader.PeekChar())) {
							// encountered a character that needs escaping
							// assume end of token
							break;
						}
						escapedValue += reader.ReadChar();
					} while (IsValidValueChar(expectedToken, reader.PeekChar()));
					((ValueNode)result).Value = escapedValue;
					break;
				case TokenType.HexString:
				case TokenType.Base58String:
				case TokenType.Number:
					result = new ValueNode {
						Type = expectedToken,
						Value = string.Empty
					};
					while (IsValidValueChar(expectedToken, reader.PeekChar())) {
						((ValueNode)result).Value = ((ValueNode)result).Value + reader.ReadChar();
					}
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(expectedToken), expectedToken, null);
			}
			if (error == EPasaErrorCode.Success && postFix != null)
				foreach (var postfixChar in postFix)
					if (!reader.MatchChar(postfixChar)) {
						error = EPasaErrorCode.BadFormat;
						return null;
					}
			return result;
		}


		private bool IsStartChar(TokenType token, char? character) {
			if (character == null)
				return false;
			var c = (char)character;
			switch (token) {
				case TokenType.EPASA:
					return IsStartChar(TokenType.PASA, c);
				case TokenType.PASA:
					return IsStartChar(TokenType.AccountName, c) || IsStartChar(TokenType.AccountNumber, c);
				case TokenType.AccountName:
					return IsStartChar(TokenType.Pascal64String, c);
				case TokenType.AccountNumber:
					return char.IsDigit((char)c);
				case TokenType.Checksum:
					return c == '-';
				case TokenType.Payload:
					return c.IsIn('[', '(', '<', '{');
				case TokenType.PayloadContent:
					return c.IsIn('0', '"') || IsStartChar(TokenType.Base58String, c);
				case TokenType.ExtendedChecksum:
					return HexEncoding.HexStringCharSet.Contains($"{c}");
				case TokenType.Number:
					return char.IsDigit(c);
				case TokenType.PascalAsciiString:
					return PascalAsciiEncoding.PascalAsciiCharSetUnescaped.Contains($"{c}") || c == PascalAsciiEncoding.EscapeChar;
				case TokenType.Pascal64String:
					return Pascal64Encoding.Pascal64CharSetStart.Contains($"{c}") || c == Pascal64Encoding.EscapeChar;
				case TokenType.HexString:
					return HexEncoding.HexStringCharSet.Contains($"{c}");
				case TokenType.Base58String:
					return PascalBase58Encoding.CharSet.Contains($"{c}");
				default:
					throw new ArgumentOutOfRangeException(nameof(token), token, null);
			}
		}

		private bool IsValidValueChar(TokenType token, char? character) {
			if (character == null)
				return false;
			var c = (char)character;
			switch (token) {
				case TokenType.EPASA:
				case TokenType.PASA:
				case TokenType.AccountName:
				case TokenType.AccountNumber:
				case TokenType.Payload:
				case TokenType.PayloadContent:
				case TokenType.ExtendedChecksum:
					throw new ArgumentOutOfRangeException(nameof(token), token, "Token not a grammar terminal");
				case TokenType.Number:
					return char.IsDigit(c);
				case TokenType.PascalAsciiString:
					return PascalAsciiEncoding.PascalAsciiCharSet.Contains(c.ToString()) || c == PascalAsciiEncoding.EscapeChar;
				case TokenType.Pascal64String:
					return Pascal64Encoding.Pascal64CharSet.Contains(c.ToString()) || c == Pascal64Encoding.EscapeChar;
				case TokenType.HexString:
					return HexEncoding.HexStringCharSet.Contains(c.ToString());
				case TokenType.Base58String:
					return PascalBase58Encoding.CharSet.Contains(c.ToString());
				default:
					throw new ArgumentOutOfRangeException(nameof(token), token, null);
			}
		}

		private char GetEscapeChar(TokenType token) {
			switch (token) {
				case TokenType.PascalAsciiString:
					return PascalAsciiEncoding.EscapeChar;
				case TokenType.Pascal64String:
					return Pascal64Encoding.EscapeChar;
			}
			throw new ArgumentOutOfRangeException(nameof(token), token, null);
		}

		private bool IsEscapedChar(TokenType token, char? c) {
			if (c == null)
				return false;
			switch (token) {
				case TokenType.PascalAsciiString:
					return PascalAsciiEncoding.PascalAsciiCharSetEscaped.Contains((char)c);
				case TokenType.Pascal64String:
					return Pascal64Encoding.Pascal64CharSetEscaped.Contains((char)c);
			}
			throw new ArgumentOutOfRangeException(nameof(token), token, null);
		}

		private EPasaErrorCode TryCompile(EPasaNode epasaNode, EPasa epasa) {

			EPasaErrorCode TryCompileAccount(AccountNode accountNode) {
				switch (accountNode.Type) {
					case TokenType.AccountName:
						if (!Pascal64Encoding.IsValidEscaped(accountNode.Name.Value))
							return EPasaErrorCode.InvalidAccountName;
						epasa.AccountName = Pascal64Encoding.Unescape(accountNode.Name.Value);
						epasa.PayloadType = epasa.PayloadType.SetFlags(PayloadType.AddressedByName);
						break;
					case TokenType.AccountNumber:
						if (!uint.TryParse(accountNode.AccountNo.Value, out var accNo))
							return EPasaErrorCode.BadFormat;
						if (accountNode.AccountNo.Value.StartsWith("0") && accNo != 0) {
							return EPasaErrorCode.BadFormat;
						}
						epasa.Account = accNo;
						var actualChecksum = AccountHelper.CalculateAccountChecksum(accNo);
						if (accountNode.Checksum != null) {
							if (!uint.TryParse(accountNode.Checksum.Value, out var checksum))
								return EPasaErrorCode.BadFormat;
							if (checksum != actualChecksum)
								return EPasaErrorCode.AccountChecksumInvalid;
						}
						epasa.AccountChecksum = actualChecksum;
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(accountNode));
				}
				return EPasaErrorCode.Success;
			}

			EPasaErrorCode TryCompilePayload(PayloadNode payloadNode) {
				epasa.PayloadType = epasa.PayloadType | payloadNode.Encryption;
				if (payloadNode.Content != null) {
					switch (payloadNode.Content.Type) {
						case TokenType.PascalAsciiString:
							epasa.PayloadType = epasa.PayloadType | PayloadType.AsciiFormatted;
							epasa.Payload = PascalAsciiEncoding.Unescape(payloadNode.Content.Value);
							break;
						case TokenType.Base58String:
							epasa.PayloadType = epasa.PayloadType | PayloadType.Base58Formatted;
							epasa.Payload = payloadNode.Content.Value;
							break;
						case TokenType.HexString:
							epasa.PayloadType = epasa.PayloadType | PayloadType.HexFormatted;
							epasa.Payload = payloadNode.Content.Value;
							break;
						default:
							throw new ArgumentOutOfRangeException(nameof(payloadNode.Content.Type));
					}					
					if (!EPasaHelper.IsValidPayloadLength(epasa.PayloadType, epasa.Payload)) {
						return EPasaErrorCode.PayloadTooLarge;
					}
				}
				if (payloadNode.Password != null) {
					if (!EPasaHelper.IsValidPasswordLength(payloadNode.Password.Value)) {
						return EPasaErrorCode.InvalidPassword;
					}
					epasa.Password = PascalAsciiEncoding.Unescape(payloadNode.Password.Value);
				}
				return EPasaErrorCode.Success;
			}

			EPasaErrorCode TryCompileExtendedChecksum(ValueNode extendedChecksumNode) {
				var actualExtendedChecksum = EPasaHelper.ComputeExtendedChecksum( epasa.ToString(true) );
				if (extendedChecksumNode != null && extendedChecksumNode.Value != actualExtendedChecksum) {
					return EPasaErrorCode.AccountChecksumInvalid;
				}
				epasa.ExtendedChecksum = actualExtendedChecksum;
				return EPasaErrorCode.Success;
			}

			var result = TryCompileAccount(epasaNode.Account);
			if (result != EPasaErrorCode.Success)
				return result;
			if (epasaNode.Payload != null) {
				result = TryCompilePayload(epasaNode.Payload);
				if (result != EPasaErrorCode.Success)
					return result;
			}
			result = TryCompileExtendedChecksum(epasaNode.ExtendedChecksum);
			if (result != EPasaErrorCode.Success)
				return result;

			return result;

		}
	}
}

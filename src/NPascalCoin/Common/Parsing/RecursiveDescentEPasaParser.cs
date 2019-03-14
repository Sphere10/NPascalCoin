using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NPascalCoin.API.Classic.Objects;
using NPascalCoin.Common;
using Sphere10.Framework;


namespace NPascalCoin.Common.Parsing {

	public class RecursiveDescentEPasaParser : IEPasaParser {

		public enum TokenType {
			EPASA,
			PASA,
			AccountName,
			AccountNumber,
			Checksum,
			ExtendedAddress,
			PublicPayload,
			ReceiverEncPayload,
			SenderEncPayload,
			PasswordEncPayload,
			Payload,
			PayloadChecksum,
			Number,
			SafeAnsiString,
			Pascal64String,
			HexString,
			Base58String
		}

		public abstract class Node {
			public TokenType Type { get; set; }
		}

		public sealed class EPasaNode : Node {
			public AccountNode Account;
			public ExtendedAddressNode Extension;
		}

		public sealed class AccountNode : Node {
			public ValueNode AccountNo;
			public ValueNode Checksum;
			public ValueNode Name { get; set; }
		}

		public sealed class ExtendedAddressNode : Node {
			public PayloadNode Payload;
			public ValueNode PayloadChecksum;
		}

		public sealed class PayloadNode : Node {
			public PayloadType Encryption { get; set; }
			public ValueNode Password { get; set; }
			public ValueNode Content { get; set; }
		}

		public sealed class ValueNode : Node {
			public string Value { get; set; }
		}


		public bool TryParse(string epasaText, out EPasa epasa, out EPasaErrorCode errorCode) {
			throw new NotImplementedException();
		}

		public Node TryParseEPasaInternal(TokenType expectedToken, TextReader reader, ref string error, string prefix = null, string postFix = null) {
			Node result = null;

			if (error != null)
				return null;

			if (prefix != null)
				foreach (var prefixChar in prefix)
					if (!SkipChar(reader, prefixChar, ref error))
						return null;

			var nextChar = NextChar(reader);
			if (nextChar == null) {
				error = "Unexpected termination";
				return null;
			}

			switch (expectedToken) {
				case TokenType.EPASA:
					result = new EPasaNode {
						Type = TokenType.EPASA,
						Account = TryParseEPasaInternal(TokenType.PASA, reader, ref error) as AccountNode
					};
					break;
				case TokenType.PASA:
					if (IsStartChar(TokenType.AccountNumber, nextChar)) {
						result = TryParseEPasaInternal(TokenType.AccountNumber, reader, ref error);
					} else if (IsStartChar(TokenType.AccountName, nextChar)) {
						result = TryParseEPasaInternal(TokenType.AccountName, reader, ref error);
					} else {
						error = "Invalidly formatted account";
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
						Checksum = TryParseEPasaInternal(TokenType.Number, reader, ref error, prefix: "-") as ValueNode
					};
					break;
				case TokenType.Checksum:
					//result = T
					break;
				case TokenType.ExtendedAddress:
					result = new ExtendedAddressNode {
						Type = TokenType.ExtendedAddress
					};
					if (IsStartChar(TokenType.PublicPayload, nextChar)) {
						((ExtendedAddressNode) result).Payload = TryParseEPasaInternal(TokenType.Payload, reader, ref error) as PayloadNode;
					} else if (IsStartChar(TokenType.ReceiverEncPayload, nextChar)) {
						((ExtendedAddressNode) result).Payload = TryParseEPasaInternal(TokenType.ReceiverEncPayload, reader, ref error) as PayloadNode;
					} else if (IsStartChar(TokenType.SenderEncPayload, nextChar)) {
						((ExtendedAddressNode) result).Payload = TryParseEPasaInternal(TokenType.SenderEncPayload, reader, ref error) as PayloadNode;
					} else if (IsStartChar(TokenType.PasswordEncPayload, nextChar)) {
						((ExtendedAddressNode) result).Payload = TryParseEPasaInternal(TokenType.PasswordEncPayload, reader, ref error) as PayloadNode;
					} else {
						error = "Invalid extended address";
					}

					if (error != null) {
						nextChar = NextChar(reader);
						if (nextChar != null && nextChar == ':') {
							((ExtendedAddressNode) result).PayloadChecksum = TryParseEPasaInternal(TokenType.HexString, reader, ref error, prefix: ":") as ValueNode;
						}
					}

					break;
				case TokenType.PublicPayload:
					SkipChar(reader, '[', ref error);
					result = TryParseEPasaInternal(TokenType.Payload, reader, ref error);
					((PayloadNode) result).Encryption = ((PayloadNode) result).Encryption | PayloadType.Public;
					SkipChar(reader, ']', ref error);
					break;
				case TokenType.ReceiverEncPayload:
					SkipChar(reader, '(', ref error);
					result = TryParseEPasaInternal(TokenType.Payload, reader, ref error);
					((PayloadNode) result).Encryption = ((PayloadNode)result).Encryption | PayloadType.RecipientKeyEncrypted;
					SkipChar(reader, ')', ref error);
					break;
				case TokenType.SenderEncPayload:
					SkipChar(reader, '<', ref error);
					result = TryParseEPasaInternal(TokenType.Payload, reader, ref error);
					((PayloadNode) result).Encryption = ((PayloadNode)result).Encryption | PayloadType.SenderKeyEncrypted;
					SkipChar(reader, '>', ref error);
					break;
				case TokenType.PasswordEncPayload:
					SkipChar(reader, '{', ref error);
					result = TryParseEPasaInternal(TokenType.Payload, reader, ref error);
					((PayloadNode) result).Encryption = ((PayloadNode)result).Encryption | PayloadType.AESEncrypted;
					SkipChar(reader, ':', ref error);
					((PayloadNode) result).Password = TryParseEPasaInternal(TokenType.SafeAnsiString, reader, ref error) as ValueNode;
					SkipChar(reader, '}', ref error);
					break;
				case TokenType.Payload:
					if (nextChar == '"') {
						result = new PayloadNode {
							Type = TokenType.SafeAnsiString,
							Content = TryParseEPasaInternal(TokenType.SafeAnsiString, reader, ref error, prefix: "\"", postFix: "\"") as ValueNode
						};
					} else if (IsStartChar(TokenType.HexString, nextChar)) {
						result = new PayloadNode {
							Type = TokenType.HexString,
							Content = TryParseEPasaInternal(TokenType.HexString, reader, ref error, prefix: "0x") as ValueNode
						};
					} else if (IsStartChar(TokenType.Base58String, nextChar)) {
						result = new PayloadNode {
							Type = TokenType.Base58String,
							Content = TryParseEPasaInternal(TokenType.Base58String, reader, ref error) as ValueNode
						};
					}

					break;
				case TokenType.PayloadChecksum:
					result = TryParseEPasaInternal(TokenType.HexString, reader, ref error);
					break;
				case TokenType.SafeAnsiString:
					string safeAnsiString = null;
					while (IsValidValueChar(TokenType.SafeAnsiString, nextChar)) {
						if (nextChar == EncodingHelper.EscapeChar) {
							nextChar = NextChar(reader);
							if (nextChar == null || !EncodingHelper.SafeAnsiCharSetEscaped.Contains($"{nextChar}")) {
								error = nextChar == null ? "Unexpected end" : $"Illegal escape sequence \\'{nextChar}'";
								break;
							}
						}
						safeAnsiString = (safeAnsiString ?? string.Empty) + nextChar;
						nextChar = NextChar(reader);
					}

					if (error != null)
						result = new ValueNode {
							Type = TokenType.SafeAnsiString,
							Value = safeAnsiString
						};
					break;
				case TokenType.Pascal64String:
				case TokenType.HexString:
				case TokenType.Base58String:

					string value = null;
					while (IsValidValueChar(expectedToken, nextChar)) {
						value = (value ?? string.Empty) + nextChar;
						nextChar = NextChar(reader);
					}

					if (error != null)
						result = new ValueNode {
							Type = expectedToken,
							Value = value
						};
					break;
				case TokenType.Number:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(expectedToken), expectedToken, null);
			}

			if (error != null && postFix != null)
				foreach (var postfixChar in postFix)
					if (!SkipChar(reader, postfixChar, ref error))
						return null;
			return result;
		}

		public char? NextChar(TextReader reader) {
			var nextValue = reader.Peek();
			if (nextValue == -1)
				return null;
			return (char) nextValue;
		}

		public bool SkipChar(TextReader reader, char? character, ref string error) {
			if (error != null)
				return false;
			var nextValue = reader.Peek();
			if (nextValue == -1) {
				if (character != null) {
					error = "Unexpected termination";
					return false;
				} else {
					return true;
				}
			}

			if ((char) nextValue != character) {
				error = $"Expected '{character}' but found '{(char) nextValue}'";
				return false;
			}

			reader.Read();
			return true;
		}

		public bool IsStartChar(TokenType token, char? character) {
			if (character == null)
				return false;
			var c = (char) character;
			switch (token) {
				case TokenType.EPASA:
					return IsStartChar(TokenType.PASA, c);
				case TokenType.PASA:
					return IsStartChar(TokenType.AccountName, c) || IsStartChar(TokenType.AccountNumber, c);
				case TokenType.AccountName:
					return IsStartChar(TokenType.Pascal64String, c);
				case TokenType.AccountNumber:
					return char.IsDigit((char) c) && c != '0';
				case TokenType.Checksum:
					return char.IsDigit((char) c);
				case TokenType.ExtendedAddress:
					return c.IsIn('[', '(', '<', '{');
				case TokenType.PublicPayload:
					return c == '[';
				case TokenType.ReceiverEncPayload:
					return c == '(';
				case TokenType.SenderEncPayload:
					return c == '<';
				case TokenType.PasswordEncPayload:
					return c == '{';
				case TokenType.Payload:
					return c.IsIn('0', '"') || IsStartChar(TokenType.Base58String, c);
				case TokenType.PayloadChecksum:
					return EncodingHelper.HexStringCharSet.Contains($"{c}");
				case TokenType.Number:
					return c.IsIn('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
				case TokenType.SafeAnsiString:
					return EncodingHelper.SafeAnsiCharSetUnescaped.Contains($"{c}");
				case TokenType.Pascal64String:
					return EncodingHelper.Pascal64CharSet.Contains($"{c}");
				case TokenType.HexString:
					return EncodingHelper.HexStringCharSet.Contains($"{c}");
				case TokenType.Base58String:
					return EncodingHelper.Base58CharSet.Contains($"{c}");
				default:
					throw new ArgumentOutOfRangeException(nameof(token), token, null);
			}
		}

		public bool IsValidValueChar(TokenType token, char? character) {
			if (character == null)
				return false;
			var c = (char) character;
			switch (token) {
				case TokenType.EPASA:
				case TokenType.PASA:
				case TokenType.AccountName:
				case TokenType.AccountNumber:
				case TokenType.Checksum:
				case TokenType.ExtendedAddress:
				case TokenType.PublicPayload:
				case TokenType.ReceiverEncPayload:
				case TokenType.SenderEncPayload:
				case TokenType.PasswordEncPayload:
				case TokenType.Payload:
				case TokenType.PayloadChecksum:
					throw new ArgumentOutOfRangeException(nameof(token), token, "Token not a grammar terminal");
				case TokenType.Number:
					return c.IsIn('1', '2', '3', '4', '5', '6', '7', '8', '9');
				case TokenType.SafeAnsiString:
					return (int) c >= 32 && (int) c <= 126;
				case TokenType.Pascal64String:
					return EncodingHelper.Pascal64CharSet.Contains(c.ToString());
				case TokenType.HexString:
					return EncodingHelper.HexStringCharSet.Contains(c.ToString());
				case TokenType.Base58String:
					return EncodingHelper.Base58CharSet.Contains(c.ToString());
				default:
					throw new ArgumentOutOfRangeException(nameof(token), token, null);
			}
		}

	}
}

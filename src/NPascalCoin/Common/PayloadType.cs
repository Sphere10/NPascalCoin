using System;

namespace NPascalCoin.Common {

	[Flags]
	public enum PayloadType {

		/// <summary>
		/// Payload encryption and encoding method not specified.
		/// </summary>
		NonDeterministic = 0x00000000,

		/// <summary>
		/// Unencrypted, public payload.
		/// </summary>
		Public = 0x00000001,

		/// <summary>
		/// ECIES encrypted using recipient accounts public key.
		/// </summary>		
		RecipientKeyEncrypted = 0x00000010,

		/// <summary>
		/// ECIES encrypted using sender accounts public key.
		/// </summary>
		SenderKeyEncrypted = 0x00000100,

		/// <summary>
		/// AES encrypted using pwd param
		/// </summary>
		PasswordEncrypted = 0x00001000,

		/// <summary>
		/// Payload data encoded in ASCII
		/// </summary>
		AsciiFormatted = 0x00010000,

		/// <summary>
		/// Payload data encoded in HEX
		/// </summary>
		HexFormatted = 0x00100000,

		/// <summary>
		/// Payload data encoded in Base58
		/// </summary>
		Base58Formatted = 0x01000000,

		/// <summary>
		/// E-PASA addressed by account name (not number).
		/// </summary>
		AddressedByName = 0x10000000,

	}
}
using System;

namespace NPascalCoin.Common {

	[Flags]
	public enum PayloadType {

		/// <summary>
		/// Payload encoding method not specified.
		/// </summary>
		NonDeterministic = 0x00000000,

		/// <summary>
		/// Not encrypted public payload.
		/// </summary>
		Public = 0x00000001,

		/// <summary>
		/// Encrypted using recipient accounts public key.
		/// </summary>		
		RecipientKeyEncrypted = 0x00000010,

		/// <summary>
		/// Encrypted using sender accounts public key.
		/// </summary>
		SenderKeyEncrypted = 0x00000100,

		/// <summary>
		/// Encrypted data using pwd param
		/// </summary>
		AESEncrypted = 0x00001000,

		/// <summary>
		/// Payload data is ASCII
		/// </summary>
		AsciiFormatted = 0x0001000,

		/// <summary>
		/// Payload data is HEX
		/// </summary>
		HexFormatted = 0x0010000,

		/// <summary>
		/// Payload data is Base58
		/// </summary>
		Base58Formatted = 0x0100000,

		/// <summary>
		/// E-Pasa encoding uses account name
		/// </summary>
		AddressedByName = 0x1000000,

	}
}
using System;

namespace NPascalCoin.Payloads {

	[Flags]
	public enum PayloadType : byte {

		/// <summary>
		/// Payload encryption and encoding method not specified.
		/// </summary>
		NonDeterministic        = 0,

		/// <summary>
		/// Unencrypted, public payload.
		/// </summary>
		Public                  = 1 << 0,

		/// <summary>
		/// ECIES encrypted using recipient accounts public key.
		/// </summary>		
		RecipientKeyEncrypted   = 1 << 1,

		/// <summary>
		/// ECIES encrypted using sender accounts public key.
		/// </summary>
		SenderKeyEncrypted      = 1 << 2,

		/// <summary>
		/// AES encrypted using pwd param
		/// </summary>
		PasswordEncrypted       = 1 << 3,

		/// <summary>
		/// Payload data encoded in ASCII
		/// </summary>
		AsciiFormatted          = 1 << 4,

		/// <summary>
		/// Payload data encoded in HEX
		/// </summary>
		HexFormatted            = 1 << 5,

		/// <summary>
		/// Payload data encoded in Base58
		/// </summary>
		Base58Formatted         = 1 << 6,

		/// <summary>
		/// E-PASA addressed by account name (not number).
		/// </summary>
		AddressedByName         = 1 << 7,

	}
}
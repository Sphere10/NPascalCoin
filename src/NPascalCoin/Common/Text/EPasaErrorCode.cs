namespace NPascalCoin.Common.Text {
	public enum EPasaErrorCode {
		Success,
		BadFormat,
		BadChecksum,
		InvalidAccountNumber,
		AccountChecksumInvalid,
		MismatchedPayloadEncoding,
		PayloadTooLarge,
		MissingPassword,
		UnusedPassword,
		MissingAccountChecksum,
		BadExtendedChecksum
	}
}

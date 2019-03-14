namespace NPascalCoin.Common.Parsing {
	public enum EPasaErrorCode {
		Success,
		BadFormat,
		BadChecksum,
		AccountNumberTooLong,
		AccountChecksumInvalid,
		MismatchedPayloadEncoding,
		PayloadTooLarge,
		MissingPassword,
		UnusedPassword,
		MissingAccountChecksum,
		BadExtendedChecksum
	}
}

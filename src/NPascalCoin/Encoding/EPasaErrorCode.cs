namespace NPascalCoin.Encoding {
	public enum EPasaErrorCode {
		Success,
		BadFormat,
		BadChecksum,
		InvalidAccountNumber,
		AccountChecksumInvalid,
		InvalidAccountName,
		MismatchedPayloadEncoding,
		PayloadTooLarge,
		MissingPassword,
		UnusedPassword,
		InvalidPassword,
		BadExtendedChecksum,				
	}
}

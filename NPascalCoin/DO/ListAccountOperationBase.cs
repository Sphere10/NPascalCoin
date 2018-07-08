namespace NPascalCoin {
	public class ListAccountOperationBase : Operation {
		public virtual ulong AccountSigner { get; set; }
		public virtual ulong AccountTarget { get; set; }
		public virtual ListAccountType ListType { get; set; }
		public virtual ulong NOperation { get; set; }
		public virtual ulong AccountPrice { get; set; }
		public virtual ulong AccountToPay { get; set; }
		public virtual ulong Fee { get; set; }
		public virtual byte[] Payload { get; set; }
		public virtual PublicKey PublicKey { get; set; }
		public virtual PublicKey NewPublicKey { get; set; }   // If EC_OpenSSL_NID=0 then is OPEN, otherwise is for only 1 public key
		public virtual ulong LockedUntilBlock { get; set; }
		public virtual Signature Signature { get; set; }
	}
}
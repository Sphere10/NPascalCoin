namespace NPascalCoin {
	public class ChangeKeyOperation : Operation {
		public virtual ulong AccountSigner { get; set; }
		public virtual ulong AccountTarget { get; set; }
		public virtual ulong NOperation { get; set; }
		public virtual ulong Fee { get; set; }
		public virtual byte[] Payload { get; set; }
		public virtual PublicKey PublicKey { get; set; }
		public virtual PublicKey NewAccountKey { get; set; }
		public virtual Signature Signature { get; set; }
	}
}
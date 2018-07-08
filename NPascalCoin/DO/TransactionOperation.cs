namespace NPascalCoin {
	public class TransactionOperation : Operation {
		public virtual ulong Sender { get; set; } 
		public virtual ulong NOperation { get; set; }
		public virtual ulong Target { get; set; }
		public virtual ulong Amount { get; set; }
		public virtual ulong Fee { get; set; }
		public virtual byte[] Payload { get; set; }
		public virtual PublicKey PublicKey { get; set; }
		public virtual Signature Signature { get; set; }
		// Protocol 2
		// Next values will only be filled after this operation is executed
		public virtual TransactionStyle Style { get; set; }
		public virtual ulong AccountPrice { get; set; }
		public virtual ulong SellerAccount { get; set; }
		public virtual PublicKey NewAccountKey { get; set; }
	}
}
namespace NPascalCoin {
	public class ChangeAccountInfo : Operation {
		public virtual ulong AccountSigner { get; set; }
		public virtual ulong AccountTarget { get; set; }
		public virtual ulong NOperation { get; set; }
		public virtual ulong Fee { get; set; }
		public virtual byte[] Payload { get; set; }
		public virtual PublicKey PublicKey { get; set; }
		public virtual ChangeInfoType ChangesType { get; set; }
		public virtual PublicKey NewAccountkey { get; set; }
		public virtual byte[] NewName { get; set; }
		public virtual ushort NewType { get; set; }
		public virtual Signature Signature { get; set; }
	}
}
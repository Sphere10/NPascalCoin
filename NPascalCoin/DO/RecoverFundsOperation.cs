namespace NPascalCoin {
	public class RecoverFundsOperation : Operation {
		public virtual ulong Account { get; set; }
		public virtual ulong NOperation { get; set; }
		public virtual ulong Fee { get; set; }
	}
}
namespace NPascalCoin {
	public class MultiOperationSender {
		public virtual int DBID { get; set; }
		public virtual ulong Account { get; set; }
		public virtual ulong Amount { get; set; }
		public virtual ulong NOperation { get; set; }
		public virtual byte[] PayLoad { get; set; }
		public virtual byte[] Signature { get; set; }
	}
}
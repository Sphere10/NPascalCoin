namespace NPascalCoin {
	public class MultiOperationReceiver {
		public virtual int DBID { get; set; }
		public virtual ulong Account { get; set; }
		public virtual ulong Amount { get; set; }
		public virtual byte[] Payload { get; set; }
	}
}
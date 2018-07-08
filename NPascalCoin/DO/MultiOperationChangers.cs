namespace NPascalCoin {
	public class MultiOperationChangers {
		public virtual int DBID { get; set; }
		public virtual ulong Account { get; set; }
		public virtual ulong NOperation { get; set; }
		public virtual ChangeInfoType ChangeType { get; set; }
		public virtual PublicKey NewAccountKey { get; set; }  // If (changes_mask and $0001)=$0001 then change account key
		public virtual byte[] NewName { get; set; }          // If (changes_mask and $0002)=$0002 then change name
		public virtual ushort NewType { get; set; }               // If (changes_mask and $0004)=$0004 then change type
		public virtual ulong SellerAccount { get; set; }
		public virtual ulong AccountPrice { get; set; }
		public virtual ulong LockedUntilBlock { get; set; }
		public virtual ulong Fee { get; set; }
		public virtual Signature Signature { get; set; }
	}
}
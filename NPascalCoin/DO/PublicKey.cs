namespace NPascalCoin {
	public class PublicKey {
		public virtual int DBID { get; set; }
		public virtual short Checksum { get; set; }
		public virtual ushort EC_OpenSSL_NID { get; set; }
		public virtual byte[] X { get; set; }
		public virtual byte[] Y { get; set; }		
	}
}
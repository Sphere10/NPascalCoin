using System;

namespace NPascalCoin {
	public class PublicKey : IEquatable<PublicKey> {
		public virtual int DBID { get; set; }
		public virtual short Checksum { get; set; }
		public virtual ushort EC_OpenSSL_NID { get; set; }
		public virtual byte[] X { get; set; }
		public virtual byte[] Y { get; set; }

		public virtual bool Equals(PublicKey other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return DBID == other.DBID && Checksum == other.Checksum && EC_OpenSSL_NID == other.EC_OpenSSL_NID && Equals(X, other.X) && Equals(Y, other.Y);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((PublicKey) obj);
		}

		public override int GetHashCode() {
			unchecked {
				var hashCode = DBID;
				hashCode = (hashCode * 397) ^ Checksum.GetHashCode();
				hashCode = (hashCode * 397) ^ EC_OpenSSL_NID.GetHashCode();
				hashCode = (hashCode * 397) ^ (X != null ? X.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Y != null ? Y.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(PublicKey left, PublicKey right) {
			return Equals(left, right);
		}

		public static bool operator !=(PublicKey left, PublicKey right) {
			return !Equals(left, right);
		}
	}
}
using System;

namespace NPascalCoin {
	public class Signature : IEquatable<Signature> {
		public virtual byte[] R { get; set; }
		public virtual byte[] S { get; set; }

		public virtual bool Equals(Signature other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(R, other.R) && Equals(S, other.S);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Signature) obj);
		}

		public override int GetHashCode() {
			unchecked {
				return ((R != null ? R.GetHashCode() : 0) * 397) ^ (S != null ? S.GetHashCode() : 0);
			}
		}

		public static bool operator ==(Signature left, Signature right) {
			return Equals(left, right);
		}

		public static bool operator !=(Signature left, Signature right) {
			return !Equals(left, right);
		}
	}
}
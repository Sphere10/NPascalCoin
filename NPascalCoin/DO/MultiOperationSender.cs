using System;

namespace NPascalCoin {
	public class MultiOperationSender : IEquatable<MultiOperationSender> {
		public virtual MultiOperation Owner { get; set; }
		public virtual ulong Index { get; set; }
		public virtual ulong Account { get; set; }
		public virtual ulong Amount { get; set; }
		public virtual ulong NOperation { get; set; }
		public virtual byte[] PayLoad { get; set; }
		public virtual Signature Signature { get; set; }

		public virtual bool Equals(MultiOperationSender other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(Owner, other.Owner) && Index == other.Index && Account == other.Account && Amount == other.Amount && NOperation == other.NOperation && Equals(PayLoad, other.PayLoad) && Equals(Signature, other.Signature);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((MultiOperationSender) obj);
		}

		public override int GetHashCode() {
			unchecked {
				var hashCode = (Owner != null ? Owner.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ Index.GetHashCode();
				hashCode = (hashCode * 397) ^ Account.GetHashCode();
				hashCode = (hashCode * 397) ^ Amount.GetHashCode();
				hashCode = (hashCode * 397) ^ NOperation.GetHashCode();
				hashCode = (hashCode * 397) ^ (PayLoad != null ? PayLoad.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Signature != null ? Signature.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(MultiOperationSender left, MultiOperationSender right) {
			return Equals(left, right);
		}

		public static bool operator !=(MultiOperationSender left, MultiOperationSender right) {
			return !Equals(left, right);
		}
	}
}
using System;

namespace NPascalCoin {
	public class MultiOperationReceiver : IEquatable<MultiOperationReceiver> {
		public virtual MultiOperation Owner { get; set; }
		public virtual ulong Index { get; set; }
		public virtual ulong Account { get; set; }
		public virtual ulong Amount { get; set; }
		public virtual byte[] Payload { get; set; }

		public virtual bool Equals(MultiOperationReceiver other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(Owner, other.Owner) && Index == other.Index && Account == other.Account && Amount == other.Amount && Equals(Payload, other.Payload);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((MultiOperationReceiver) obj);
		}

		public override int GetHashCode() {
			unchecked {
				var hashCode = (Owner != null ? Owner.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ Index.GetHashCode();
				hashCode = (hashCode * 397) ^ Account.GetHashCode();
				hashCode = (hashCode * 397) ^ Amount.GetHashCode();
				hashCode = (hashCode * 397) ^ (Payload != null ? Payload.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(MultiOperationReceiver left, MultiOperationReceiver right) {
			return Equals(left, right);
		}

		public static bool operator !=(MultiOperationReceiver left, MultiOperationReceiver right) {
			return !Equals(left, right);
		}
	}
}
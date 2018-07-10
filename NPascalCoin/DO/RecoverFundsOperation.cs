using System;

namespace NPascalCoin {
	public class RecoverFundsOperation : Operation, IEquatable<RecoverFundsOperation> {
		public override OperationType Type => OperationType.RecoverFunds;
		public virtual ulong Account { get; set; }
		public virtual ulong NOperation { get; set; }
		public virtual ulong Fee { get; set; }

		public virtual bool Equals(RecoverFundsOperation other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return base.Equals(other) && Account == other.Account && NOperation == other.NOperation && Fee == other.Fee;
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((RecoverFundsOperation) obj);
		}

		public override int GetHashCode() {
			unchecked {
				int hashCode = base.GetHashCode();
				hashCode = (hashCode * 397) ^ Account.GetHashCode();
				hashCode = (hashCode * 397) ^ NOperation.GetHashCode();
				hashCode = (hashCode * 397) ^ Fee.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(RecoverFundsOperation left, RecoverFundsOperation right) {
			return Equals(left, right);
		}

		public static bool operator !=(RecoverFundsOperation left, RecoverFundsOperation right) {
			return !Equals(left, right);
		}
	}
}
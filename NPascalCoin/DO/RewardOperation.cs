using System;

namespace NPascalCoin {
	public class RewardOperation : Operation, IEquatable<RewardOperation> {
		public override OperationType Type => OperationType.BlockchainReward;
		public virtual RewardType RewardType { get; set; }
		public virtual ulong Account { get; set; }
		public virtual ulong Amount { get; set; }

		public virtual bool Equals(RewardOperation other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return base.Equals(other) && RewardType == other.RewardType && Account == other.Account && Amount == other.Amount;
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((RewardOperation) obj);
		}

		public override int GetHashCode() {
			unchecked {
				int hashCode = base.GetHashCode();
				hashCode = (hashCode * 397) ^ (int) RewardType;
				hashCode = (hashCode * 397) ^ Account.GetHashCode();
				hashCode = (hashCode * 397) ^ Amount.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(RewardOperation left, RewardOperation right) {
			return Equals(left, right);
		}

		public static bool operator !=(RewardOperation left, RewardOperation right) {
			return !Equals(left, right);
		}
	}
}
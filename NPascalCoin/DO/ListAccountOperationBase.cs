using System;

namespace NPascalCoin {
	public abstract class ListAccountOperationBase : Operation, IEquatable<ListAccountOperationBase> {
		public virtual ulong AccountSigner { get; set; }
		public virtual ulong AccountTarget { get; set; }
		public virtual ListAccountType ListType { get; set; }
		public virtual ulong NOperation { get; set; }
		public virtual ulong AccountPrice { get; set; }
		public virtual ulong AccountToPay { get; set; }
		public virtual ulong Fee { get; set; }
		public virtual byte[] Payload { get; set; }
		public virtual PublicKey PublicKey { get; set; }
		public virtual PublicKey NewPublicKey { get; set; }   // If EC_OpenSSL_NID=0 then is OPEN, otherwise is for only 1 public key
		public virtual ulong LockedUntilBlock { get; set; }
		public virtual Signature Signature { get; set; }

		public virtual bool Equals(ListAccountOperationBase other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return base.Equals(other) && AccountSigner == other.AccountSigner && AccountTarget == other.AccountTarget && ListType == other.ListType && NOperation == other.NOperation && AccountPrice == other.AccountPrice && AccountToPay == other.AccountToPay && Fee == other.Fee && Equals(Payload, other.Payload) && Equals(PublicKey, other.PublicKey) && Equals(NewPublicKey, other.NewPublicKey) && LockedUntilBlock == other.LockedUntilBlock && Equals(Signature, other.Signature);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((ListAccountOperationBase) obj);
		}

		public override int GetHashCode() {
			unchecked {
				int hashCode = base.GetHashCode();
				hashCode = (hashCode * 397) ^ AccountSigner.GetHashCode();
				hashCode = (hashCode * 397) ^ AccountTarget.GetHashCode();
				hashCode = (hashCode * 397) ^ (int) ListType;
				hashCode = (hashCode * 397) ^ NOperation.GetHashCode();
				hashCode = (hashCode * 397) ^ AccountPrice.GetHashCode();
				hashCode = (hashCode * 397) ^ AccountToPay.GetHashCode();
				hashCode = (hashCode * 397) ^ Fee.GetHashCode();
				hashCode = (hashCode * 397) ^ (Payload != null ? Payload.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (PublicKey != null ? PublicKey.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (NewPublicKey != null ? NewPublicKey.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ LockedUntilBlock.GetHashCode();
				hashCode = (hashCode * 397) ^ (Signature != null ? Signature.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(ListAccountOperationBase left, ListAccountOperationBase right) {
			return Equals(left, right);
		}

		public static bool operator !=(ListAccountOperationBase left, ListAccountOperationBase right) {
			return !Equals(left, right);
		}
	}
}